// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
using System.Threading.Tasks;
using FurEver_Together.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurEver_Together.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public EmailModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userNotFoundResult = HandleUserNotFound(user);
            return userNotFoundResult ?? await LoadAndReturnPage(user);
        }

        private IActionResult HandleUserNotFound(User user)
        {
            return user == null
                ? NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.")
                : null;
        }

        private async Task<IActionResult> LoadAndReturnPage(User user)
        {
            await LoadUserEmailAsync(user);
            return Page();
        }

        private async Task LoadUserEmailAsync(User user)
        {
            Email = await _userManager.GetEmailAsync(user);
            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }
    }
}