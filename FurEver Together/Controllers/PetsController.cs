using System;
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

namespace FurEver_Together.Controllers
{
    public class PetsController : Controller
    {
        private readonly IPetService _petService;
        private readonly IMatchingService _matchingService;
        private readonly UserManager<User> _userManager;
        private readonly IUserProfileService _userProfileService;

        public PetsController(
            IPetService petService,
            IMatchingService matchingService,
            UserManager<User> userManager,
            IUserProfileService userProfileService)
        {
            _petService = petService;
            _matchingService = matchingService;
            _userManager = userManager;
            _userProfileService = userProfileService;
        }
        [HttpGet]
        [ActionName("Index")]
        public async Task<IActionResult> Index(PetType? type, string? sortOrder)
        {
            var pets = await _petService.GetAllPetsAsync();

            if (type.HasValue)
            {
                pets = pets.Where(p => p.Type == type.Value).ToList();
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder.ToLower())
                {
                    case "name_asc":
                        pets = pets.OrderBy(p => p.Name).ToList();
                        break;
                    case "name_desc":
                        pets = pets.OrderByDescending(p => p.Name).ToList();
                        break;
                }
            }

            return View(pets);
        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var pet = await _petService.GetPetByIdAsync(id.Value);
            if (pet == null)
                return NotFound();

            double matchPercentage = 0;

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                matchPercentage = await _matchingService.GetMatchPercentageForPetAndUserAsync(pet.PetId, user.Id);
            }

            ViewData["MatchPercentage"] = matchPercentage;

            return View(pet);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(PetViewModel petViewModel)
        {
            if (ModelState.IsValid)
            {
                var pet = new Pet
                {
                    Name = petViewModel.Name,
                    Type = petViewModel.Type,
                    Breed = petViewModel.Breed,
                    Age = petViewModel.Age,
                    Gender = petViewModel.Gender,
                    PictureUrl = petViewModel.PictureUrl,
                    Description = petViewModel.Description,
                    ArrivalDate = DateTime.Now,
                    IsAdopted = false,
                    Personality = new PersonalityProfile
                    {
                        EnergyLevel = petViewModel.EnergyLevel,
                        Sociability = petViewModel.Sociability,
                        AffectionLevel = petViewModel.AffectionLevel,
                        Trainability = petViewModel.Trainability,
                        Playfulness = petViewModel.Playfulness,
                        AggressionLevel = petViewModel.AggressionLevel,
                        NoiseLevel = petViewModel.NoiseLevel,
                        GoodWithKids = petViewModel.GoodWithKids,
                        GoodWithOtherPets = petViewModel.GoodWithOtherPets,
                        Adaptability = petViewModel.Adaptability,
                        AnxietyLevel = petViewModel.AnxietyLevel,
                    }
                };

                await _petService.AddPetAsync(pet);
                return RedirectToAction(nameof(Index));
            }
            return View(petViewModel);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var pet = await _petService.GetPetByIdAsync(id.Value);
            if (pet == null)
                return NotFound();

            var petViewModel = new PetViewModel
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Type = pet.Type,
                Breed = pet.Breed,
                Age = pet.Age,
                Gender = pet.Gender,
                PictureUrl = pet.PictureUrl,
                Description = pet.Description,
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
                AnxietyLevel = pet.Personality?.AnxietyLevel ?? AnxietyLevel.Calm,
            };

            return View(petViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, PetViewModel petViewModel)
        {
            if (id != petViewModel.PetId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var pet = await _petService.GetPetByIdAsync(id);
                if (pet == null)
                    return NotFound();

                pet.Name = petViewModel.Name;
                pet.Type = petViewModel.Type;
                pet.Breed = petViewModel.Breed;
                pet.Age = petViewModel.Age;
                pet.Gender = petViewModel.Gender;
                pet.PictureUrl = petViewModel.PictureUrl;
                pet.Description = petViewModel.Description;

                if (pet.Personality == null)
                    pet.Personality = new PersonalityProfile();

                pet.Personality.EnergyLevel = petViewModel.EnergyLevel;
                pet.Personality.Sociability = petViewModel.Sociability;
                pet.Personality.AffectionLevel = petViewModel.AffectionLevel;
                pet.Personality.Trainability = petViewModel.Trainability;
                pet.Personality.Playfulness = petViewModel.Playfulness;
                pet.Personality.AggressionLevel = petViewModel.AggressionLevel;
                pet.Personality.NoiseLevel = petViewModel.NoiseLevel;
                pet.Personality.GoodWithKids = petViewModel.GoodWithKids;
                pet.Personality.GoodWithOtherPets = petViewModel.GoodWithOtherPets;
                pet.Personality.Adaptability = petViewModel.Adaptability;
                pet.Personality.AnxietyLevel = petViewModel.AnxietyLevel;

                try
                {
                    await _petService.UpdatePetAsync(pet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PetExists(pet.PetId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Details", new { id = pet.PetId });
            }

            return View(petViewModel);
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
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PetExists(int id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            return pet != null;
        }
    }
}
