// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FurEver_Together.Data;
using FurEver_Together.DataModels;
using FurEver_Together.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FurEver_Together.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly FurEverTogetherDbContext _furEverTogetherDbContext;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            FurEverTogetherDbContext furEverTogetherDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _furEverTogetherDbContext = furEverTogetherDbContext;
        }

        public string Username { get; set; }
        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<Pet> Pets { get; set; }
        public List<Shelter> Shelters { get; set; }
        public PersonalityProfile? Personality { get; set; }
        public List<Adoption> UserAdoptionRequests { get; set; }
        public List<Adoption> AllAdoptionRequests { get; set; }
        public List<Volunteer> UserVolunteerApplications { get; set; }
        public List<Volunteer> AllVolunteerApplications { get; set; }
        public List<User> Users { get; set; }

        // Analytics Properties
        public PetAdoptionStats MostAdoptedAnimal { get; set; }
        public List<UserAdoptionStats> UserAdoptionStatistics { get; set; }
        public List<ShelterCapacityStats> ShelterCapacityStatistics { get; set; }
        public int TotalVolunteerRequests { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public class PetAdoptionStats
        {
            public string PetType { get; set; }
            public int TotalRequests { get; set; }
        }

        public class UserAdoptionStats
        {
            public string UserId { get; set; }
            public string UserEmail { get; set; }
            public int TotalRequests { get; set; }
            public int ApprovedRequests { get; set; }
            public double SuccessRate { get; set; }
        }

        public class ShelterCapacityStats
        {
            public string ShelterName { get; set; }
            public int Capacity { get; set; }
            public int CurrentOccupancy { get; set; }
            public double OccupancyPercentage { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };

            await LoadUserVolunteerApplications(user);
            await LoadUserPersonality(user);
        }

        private async Task LoadUserVolunteerApplications(User user)
        {
            UserVolunteerApplications = await _furEverTogetherDbContext.Volunteers
                .Where(v => v.UserId == user.Id)
                .ToListAsync();
        }

        private async Task LoadUserPersonality(User user)
        {
            var userWithPreferences = await _furEverTogetherDbContext.Users
                .Include(u => u.Preferences)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            Personality = userWithPreferences?.Preferences;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var userNotFoundResult = ValidateUser(user);
            if (userNotFoundResult != null)
                return userNotFoundResult;

            await LoadUserAdoptionRequests(user);
            await LoadUserVolunteerApplications(user);
            await LoadPets();
            await LoadShelters();
            await LoadAdminDataIfApplicable(user);
            await LoadAsync(user);

            return Page();
        }

        private IActionResult ValidateUser(User user)
        {
            return user == null
                ? NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.")
                : null;
        }

        private async Task LoadUserAdoptionRequests(User user)
        {
            UserAdoptionRequests = await _furEverTogetherDbContext.Adoptions
                .Include(a => a.Pet)
                .Where(a => a.UserId == user.Id)
                .ToListAsync();
        }

        private async Task LoadAdminDataIfApplicable(User user)
        {
            var isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");
            if (!isAdmin)
                return;

            await LoadAllAdoptionRequests();
            await LoadAllVolunteerApplications();
            await LoadAllUsers();
            await LoadAnalytics();
        }

        private async Task LoadAllAdoptionRequests()
        {
            AllAdoptionRequests = await _furEverTogetherDbContext.Adoptions
                .Include(a => a.Pet)
                .ToListAsync();
        }

        private async Task LoadAllVolunteerApplications()
        {
            AllVolunteerApplications = await _furEverTogetherDbContext.Volunteers
                .ToListAsync();
        }

        private async Task LoadAllUsers()
        {
            Users = await _userManager.Users.ToListAsync();
        }

        private async Task LoadPets()
        {
            Pets = await _furEverTogetherDbContext.Pets.ToListAsync();
        }

        private async Task LoadShelters()
        {
            Shelters = await _furEverTogetherDbContext.Shelters
                .Include(s => s.Pets)
                .ToListAsync();
        }

        private async Task LoadAnalytics()
        {
            await LoadMostAdoptedAnimal();
            await LoadUserAdoptionStatistics();
            LoadShelterCapacityStatistics();
            LoadVolunteerRequestsCount();
        }

        private async Task LoadMostAdoptedAnimal()
        {
            var adoptionsByPetType = await _furEverTogetherDbContext.Adoptions
                .Include(a => a.Pet)
                .Where(a => a.Pet != null)
                .GroupBy(a => a.Pet.Type)
                .Select(g => new PetAdoptionStats
                {
                    PetType = g.Key.ToString(),
                    TotalRequests = g.Count()
                })
                .OrderByDescending(s => s.TotalRequests)
                .FirstOrDefaultAsync();

            MostAdoptedAnimal = adoptionsByPetType ?? new PetAdoptionStats { PetType = "N/A", TotalRequests = 0 };
        }

        private async Task LoadUserAdoptionStatistics()
        {
            var userStats = await _furEverTogetherDbContext.Adoptions
                .Include(a => a.User)
                .Where(a => a.UserId != null)
                .GroupBy(a => a.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    UserEmail = g.First().User.Email,
                    TotalRequests = g.Count(),
                    ApprovedRequests = g.Count(a => a.Status == ApplicationStatus.Approved)
                })
                .ToListAsync();

            UserAdoptionStatistics = userStats.Select(s => new UserAdoptionStats
            {
                UserId = s.UserId,
                UserEmail = s.UserEmail ?? "N/A",
                TotalRequests = s.TotalRequests,
                ApprovedRequests = s.ApprovedRequests,
                SuccessRate = s.TotalRequests > 0 ? (double)s.ApprovedRequests / s.TotalRequests * 100 : 0
            }).ToList();
        }

        private void LoadShelterCapacityStatistics()
        {
            ShelterCapacityStatistics = Shelters?.Select(s => new ShelterCapacityStats
            {
                ShelterName = s.Name,
                Capacity = s.Capacity,
                CurrentOccupancy = s.Pets?.Count(p => !p.IsAdopted) ?? 0,
                OccupancyPercentage = s.Capacity > 0 
                    ? (double)(s.Pets?.Count(p => !p.IsAdopted) ?? 0) / s.Capacity * 100 
                    : 0
            }).ToList() ?? new List<ShelterCapacityStats>();
        }

        private void LoadVolunteerRequestsCount()
        {
            TotalVolunteerRequests = AllVolunteerApplications?.Count ?? 0;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var userNotFoundResult = ValidateUser(user);
            if (userNotFoundResult != null)
                return userNotFoundResult;

            var invalidModelResult = await HandleInvalidModelState(user);
            if (invalidModelResult != null)
                return invalidModelResult;

            await UpdatePhoneNumberIfChanged(user);
            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        private async Task<IActionResult> HandleInvalidModelState(User user)
        {
            if (ModelState.IsValid)
                return null;

            await LoadAsync(user);
            return Page();
        }

        private async Task UpdatePhoneNumberIfChanged(User user)
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var phoneNumberChanged = Input.PhoneNumber != phoneNumber;

            if (!phoneNumberChanged)
                return;

            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            var phoneUpdateFailed = !setPhoneResult.Succeeded;

            if (phoneUpdateFailed)
                StatusMessage = "Unexpected error when trying to set phone number.";
        }
    }
}