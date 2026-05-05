using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Wasaly.DAL.Data.Context;
using Wasaly.DAL.Enums;
using Wasaly.DAL.Models;
using Wasaly.BLL.ViewModels;

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

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public SelectList Regions { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RoleFromHome { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

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
            public string Address { get; set; }

            [Required]
            public Gender Gender { get; set; }

            [Required]
            public region Region { get; set; }

            [Required]
            [Range(10, 60)]
            public int Age { get; set; }

            [Required]
            [RegularExpression("^(01)(0|1|2|5)[0-9]{8}$", ErrorMessage = "Phone Number is not in the correct format")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadRegionsAsync()
        {
            var regions = Enum.GetValues(typeof(region))
                .Cast<region>()
                .Select(e => new
                {
                    Id = e.ToString(),
                    Name = e.ToString()
                })
                .ToList();

            Regions = new SelectList(regions, "Id", "Name");
        }

        public async Task OnGetAsync(string returnUrl = null, string roleFromHome = null)
        {
            ReturnUrl = returnUrl;

            if (!string.IsNullOrEmpty(roleFromHome))
            {
                RoleFromHome = roleFromHome;
            }

            await LoadRegionsAsync();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.PhoneNumber = Input.PhoneNumber;
                user.Location = new Location { Address = Input.Address };
                user.gender = Input.Gender;
                user.FullName = Input.FullName;
                user.Age = Input.Age;
                user.Region = Input.Region;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    var allowedRoles = new[] { "Merchant", "Courier" };

                    if (string.IsNullOrEmpty(RoleFromHome))
                    {
                        ModelState.AddModelError("", "الرجاء تحديد نوع الحساب (تاجر أو موصل)");
                        await LoadRegionsAsync();
                        return Page();
                    }

                    if (!allowedRoles.Contains(RoleFromHome))
                    {
                        ModelState.AddModelError("", $"نوع الحساب غير صحيح. القيم المسموحة: {string.Join(", ", allowedRoles)}");
                        await LoadRegionsAsync();
                        return Page();
                    }

                    var roleResult = await _userManager.AddToRoleAsync(user, RoleFromHome);

                    if (roleResult.Succeeded)
                    {
                        if (RoleFromHome == "Courier")
                        {
                            return RedirectToPage($"/Account/CorierRoleRegisteration", new { id = user.Id, returnUrl = "/" });
                        }
                        else if (RoleFromHome == "Merchant")
                        {
                            return RedirectToPage("/Account/MerchantRoleRegisteration", new { id = user.Id, returnUrl = "/" });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl ?? "/");
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    // عرض أخطاء إنشاء المستخدم
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            await LoadRegionsAsync();
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