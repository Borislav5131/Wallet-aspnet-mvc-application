﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Wallet.Infrastructure.Data.Models;

using static Wallet.Core.Constants.UserConstants.Roles;
using static Wallet.Infrastructure.Data.DataConstants.User;

namespace Wallet.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _notyf;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            INotyfService notyf)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _notyf = notyf;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(UserNameMaxLenght, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = UserNameMinLenght)]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(PasswordMaxLenght, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = PasswordMinLenght)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(IFormFile image)
        {
            string returnUrl = Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = Input.UserName,
                    Email = Input.Email,
                    Image = ConvertImageToBytes(image),
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(UserRole))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRole));
                    }

                    await _userManager.AddToRoleAsync(user, UserRole);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _notyf.Success("Successfully register!");
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private byte[] ConvertImageToBytes(IFormFile image)
        {
            var ms = new MemoryStream();
            image.CopyTo(ms);

            return ms.ToArray();
        }
    }
}
