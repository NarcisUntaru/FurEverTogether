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

        public Volunteer VolunteerApplication { get; set; }
        public PersonalityProfile? Personality { get; set; }
        public List<Adoption> AdoptionRequests { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
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

            VolunteerApplication = await _furEverTogetherDbContext.Volunteers
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            var userWithPreferences = await _furEverTogetherDbContext.Users
        .Include(u => u.Preferences) 
        .FirstOrDefaultAsync(u => u.Id == user.Id);

            Personality = userWithPreferences?.Preferences;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            AdoptionRequests = await _furEverTogetherDbContext.Adoptions
            .Include(a => a.Pet)
            .Include(a => a.User)
            .ToListAsync();
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
