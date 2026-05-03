// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.ComponentModel.DataAnnotations;
using Wasaly.DAL.Enums;
using Wasaly.DAL.Models;

namespace Wasaly.PL.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<WasalyIdentityUser> _signInManager;
        private readonly UserManager<WasalyIdentityUser> _userManager;
        private readonly IUserStore<WasalyIdentityUser> _userStore;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RegisterModel(
            UserManager<WasalyIdentityUser> userManager,
            IUserStore<WasalyIdentityUser> userStore,
            SignInManager<WasalyIdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public SelectList Roles { get; set; }


        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Required]
            [StringLength(30, MinimumLength = 3)]
            public string FullName { get; set; } 

            [Required]
            [StringLength(200, MinimumLength = 5)]
            [RegularExpression(@"^[a-zA-Z0-9\u0600-\u06FF\s,.-]+$",
            ErrorMessage = "Address contains invalid characters")]
            public string  Address { get; set; }

            [Required]
            [RegularExpression("^(Male|Female)$")]
            public Gender Gender { get; set; }


            [Required]
            [Range(10, 60)]
            public int Age { get; set; }

            [Required]
            [RegularExpression("^(01)(0|1|2|5)[0-9]{8}$", ErrorMessage = "Phone Number is not in the correct format")]
            public string PhoneNumber { get; set; } 
            public string Role { get; set; }

        }

        private async Task LoadRolesAsync()
        {
            var roles = await Task.FromResult(_roleManager.Roles.ToList());
            Roles = new SelectList(roles, "Name", "Name");
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            LoadRolesAsync();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.PhoneNumber = Input.PhoneNumber;
                user.Location =new Location() { Address= Input.Address  } ;
                user.gender = Input.Gender;
                user.FullName = Input.FullName;
                user.Age = Input.Age;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {

                    // Validate role exists before assigning
                    if (string.IsNullOrWhiteSpace(Input.Role) || !await _roleManager.RoleExistsAsync(Input.Role))
                    {
                        ModelState.AddModelError(string.Empty, $"Selected role '{Input.Role}' does not exist.");
                        return Page();
                    }

                    result = await _userManager.AddToRoleAsync(user, Input.Role);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            // If we got this far, something failed, redisplay form
            LoadRolesAsync();
            return Page();
        }

        private WasalyIdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<WasalyIdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(WasalyIdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

       
    }
}
