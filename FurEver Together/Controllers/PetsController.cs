using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FurEver_Together.Services.Interfaces;
using FurEver_Together.DataModels;
using Microsoft.EntityFrameworkCore;
using FurEver_Together.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FurEver_Together.Enums;
using FurEverTogether.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FurEver_Together.Controllers
{
    public class PetsController : Controller
    {
        private readonly IPetService _petService;
        private readonly IShelterService _shelterService;
        private readonly IMatchingService _matchingService;
        private readonly UserManager<User> _userManager;
        private readonly IUserProfileService _userProfileService;

        public PetsController(
            IPetService petService,
            IShelterService shelterService,
            IMatchingService matchingService,
            UserManager<User> userManager,
            IUserProfileService userProfileService)
        {
            _petService = petService;
            _shelterService = shelterService;
            _matchingService = matchingService;
            _userManager = userManager;
            _userProfileService = userProfileService;
        }

        [HttpGet]
        [ActionName("Index")]
        public async Task<IActionResult> Index(PetType? type, string? sortOrder)
        {
            var pets = await GetAvailablePetsAsync();
            pets = FilterPetsByType(pets, type);

            var user = await _userManager.GetUserAsync(User);
            pets = await FilterPetsByMatchPercentageAsync(pets, user);
            pets = SortPets(pets, sortOrder);

            return View(pets);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var pet = await _petService.GetPetByIdAsync(id.Value);
            if (pet == null)
                return NotFound();

            // Load shelter data if ShelterId exists
            if (pet.ShelterId.HasValue && pet.Shelter == null)
            {
                pet.Shelter = await _shelterService.GetShelterByIdAsync(pet.ShelterId.Value);
            }

            await SetMatchPercentageViewDataAsync(pet);
            return View(pet);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            var viewModel = new PetViewModel();
            await PopulateSheltersDropdown(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(PetViewModel petViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSheltersDropdown(petViewModel);
                return View(petViewModel);
            }

            var pet = MapViewModelToPet(petViewModel);
            await _petService.AddPetAsync(pet);

            return Redirect("/Identity/Account/Manage#admin-panel");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var pet = await _petService.GetPetByIdAsync(id.Value);
            if (pet == null)
                return NotFound();

            var petViewModel = MapPetToViewModel(pet);
            await PopulateSheltersDropdown(petViewModel);
            return View(petViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, PetViewModel petViewModel)
        {
            if (id != petViewModel.PetId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await PopulateSheltersDropdown(petViewModel);
                return View(petViewModel);
            }

            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
                return NotFound();

            UpdatePetFromViewModel(pet, petViewModel);

            try
            {
                await _petService.UpdatePetAsync(pet);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PetExists(pet.PetId))
                    return NotFound();
                throw;
            }

            return RedirectToAction("Details", new { id = pet.PetId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var pet = await _petService.GetPetByIdAsync(id.Value);
            if (pet == null)
                return NotFound();

            return View(pet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _petService.DeletePetAsync(id);
            return Redirect("/Identity/Account/Manage#admin-panel");
        }

        // Private helper methods

        private async Task PopulateSheltersDropdown(PetViewModel viewModel)
        {
            var shelters = await _shelterService.GetAllSheltersAsync();
            viewModel.Shelters = shelters
                .Select(s => new SelectListItem
                {
                    Value = s.ShelterId.ToString(),
                    Text = s.Name
                })
                .ToList();

            // Add an optional "No Shelter" option
            viewModel.Shelters.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- Select a Shelter (Optional) --"
            });
        }

        private async Task<List<Pet>> GetAvailablePetsAsync()
        {
            var allPets = await _petService.GetAllPetsAsync();
            return allPets
                .Where(p => p.IsAdopted == false &&
                           p.Adoption != null &&
                           p.Adoption.Status != ApplicationStatus.Approved)
                .ToList();
        }

        private List<Pet> FilterPetsByType(List<Pet> pets, PetType? type)
        {
            if (!type.HasValue)
                return pets;

            return pets.Where(p => p.Type == type.Value).ToList();
        }

        private async Task<List<Pet>> FilterPetsByMatchPercentageAsync(List<Pet> pets, User? user)
        {
            if (user == null)
                return pets;

            var filteredPets = new List<Pet>();
            foreach (var pet in pets)
            {
                var matchPercentage = await _matchingService.GetMatchPercentageForPetAndUserAsync(pet.PetId, user.Id);
                if (matchPercentage >= 50)
                {
                    filteredPets.Add(pet);
                }
            }

            return filteredPets;
        }

        private List<Pet> SortPets(List<Pet> pets, string? sortOrder)
        {
            if (string.IsNullOrEmpty(sortOrder))
                return pets;

            return sortOrder.ToLower() switch
            {
                "name_asc" => pets.OrderBy(p => p.Name).ToList(),
                "name_desc" => pets.OrderByDescending(p => p.Name).ToList(),
                _ => pets
            };
        }

        private async Task SetMatchPercentageViewDataAsync(Pet pet)
        {
            var user = await _userManager.GetUserAsync(User);
            var matchPercentage = user != null
                ? await _matchingService.GetMatchPercentageForPetAndUserAsync(pet.PetId, user.Id)
                : 0;

            ViewData["MatchPercentage"] = matchPercentage;
        }

        private Pet MapViewModelToPet(PetViewModel viewModel)
        {
            return new Pet
            {
                Name = viewModel.Name,
                Type = viewModel.Type,
                Breed = viewModel.Breed,
                Age = viewModel.Age,
                Gender = viewModel.Gender,
                PictureUrl = viewModel.PictureUrl,
                Description = viewModel.Description,
                ArrivalDate = DateTime.Now,
                IsAdopted = false,
                ShelterId = viewModel.ShelterId,
                Personality = CreatePersonalityProfile(viewModel)
            };
        }

        private PersonalityProfile CreatePersonalityProfile(PetViewModel viewModel)
        {
            return new PersonalityProfile
            {
                EnergyLevel = viewModel.EnergyLevel,
                Sociability = viewModel.Sociability,
                AffectionLevel = viewModel.AffectionLevel,
                Trainability = viewModel.Trainability,
                Playfulness = viewModel.Playfulness,
                AggressionLevel = viewModel.AggressionLevel,
                NoiseLevel = viewModel.NoiseLevel,
                GoodWithKids = viewModel.GoodWithKids,
                GoodWithOtherPets = viewModel.GoodWithOtherPets,
                Adaptability = viewModel.Adaptability,
                AnxietyLevel = viewModel.AnxietyLevel
            };
        }

        private PetViewModel MapPetToViewModel(Pet pet)
        {
            return new PetViewModel
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Type = pet.Type,
                Breed = pet.Breed,
                Age = pet.Age,
                Gender = pet.Gender,
                PictureUrl = pet.PictureUrl,
                Description = pet.Description,
                ShelterId = pet.ShelterId,
                EnergyLevel = pet.Personality?.EnergyLevel ?? EnergyLevel.Low,
                Sociability = pet.Personality?.Sociability ?? Sociability.Shy,
                AffectionLevel = pet.Personality?.AffectionLevel ?? AffectionLevel.Independent,
                Trainability = pet.Personality?.Trainability ?? Trainability.Difficult,
                Playfulness = pet.Personality?.Playfulness ?? Playfulness.Low,
                AggressionLevel = pet.Personality?.AggressionLevel ?? AggressionLevel.None,
                NoiseLevel = pet.Personality?.NoiseLevel ?? NoiseLevel.Quiet,
                GoodWithKids = pet.Personality?.GoodWithKids ?? Question.No,
                GoodWithOtherPets = pet.Personality?.GoodWithOtherPets ?? Question.No,
                Adaptability = pet.Personality?.Adaptability ?? Adaptability.NeedsRoutine,
                AnxietyLevel = pet.Personality?.AnxietyLevel ?? AnxietyLevel.Calm
            };
        }

        private void UpdatePetFromViewModel(Pet pet, PetViewModel viewModel)
        {
            pet.Name = viewModel.Name;
            pet.Type = viewModel.Type;
            pet.Breed = viewModel.Breed;
            pet.Age = viewModel.Age;
            pet.Gender = viewModel.Gender;
            pet.PictureUrl = viewModel.PictureUrl;
            pet.Description = viewModel.Description;
            pet.ShelterId = viewModel.ShelterId;

            pet.Personality ??= new PersonalityProfile();
            UpdatePersonalityProfile(pet.Personality, viewModel);
        }

        private void UpdatePersonalityProfile(PersonalityProfile personality, PetViewModel viewModel)
        {
            personality.EnergyLevel = viewModel.EnergyLevel;
            personality.Sociability = viewModel.Sociability;
            personality.AffectionLevel = viewModel.AffectionLevel;
            personality.Trainability = viewModel.Trainability;
            personality.Playfulness = viewModel.Playfulness;
            personality.AggressionLevel = viewModel.AggressionLevel;
            personality.NoiseLevel = viewModel.NoiseLevel;
            personality.GoodWithKids = viewModel.GoodWithKids;
            personality.GoodWithOtherPets = viewModel.GoodWithOtherPets;
            personality.Adaptability = viewModel.Adaptability;
            personality.AnxietyLevel = viewModel.AnxietyLevel;
        }

        private async Task<bool> PetExists(int id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            return pet != null;
        }
    }
}