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
            [Required(ErrorMessage = "هذا الحقل مطلوب")]
            [EmailAddress(ErrorMessage = "يرجى إدخال بريد إلكتروني صحيح")]
            [Display(Name = "البريد الإلكتروني")]
            public string Email { get; set; }

            [Required(ErrorMessage = "هذا الحقل مطلوب")]
            [StringLength(100, MinimumLength = 6,
                ErrorMessage = "يجب أن تكون كلمة المرور بين 6 و 100 حرف")]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور")]
            public string Password { get; set; }

            [Required(ErrorMessage = "هذا الحقل مطلوب")]
            [DataType(DataType.Password)]
            [Display(Name = "تأكيد كلمة المرور")]
            [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "هذا الحقل مطلوب")]
            [StringLength(30, MinimumLength = 3,
                ErrorMessage = "يجب أن يكون الاسم بين 3 و 30 حرف")]
            [Display(Name = "الاسم بالكامل")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "هذا الحقل مطلوب")]
            [StringLength(200, MinimumLength = 5)]
         
            [Display(Name = "العنوان")]
            // في InputModel
            public string Address { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            [Required(ErrorMessage = "هذا الحقل مطلوب")]
            [Display(Name = "النوع")]
            public Gender Gender { get; set; }

            [Required(ErrorMessage = "هذا الحقل مطلوب")]
            [Display(Name = "المنطقة")]
            public region Region { get; set; }

            [Required(ErrorMessage = "هذا الحقل مطلوب")]
            [Range(10, 60, ErrorMessage = "العمر يجب أن يكون بين 10 و 60 سنة")]
            [Display(Name = "العمر")]
            public int Age { get; set; }

            [Required(ErrorMessage = "هذا الحقل مطلوب")]
            [RegularExpression("^(01)(0|1|2|5)[0-9]{8}$",
                ErrorMessage = "رقم الهاتف يجب أن يكون 11 رقم ويبدأ بـ 010 أو 011 أو 012 أو 015")]
            [Display(Name = "رقم الهاتف")]
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
                //var Admin = CreateUser();
                //user.PhoneNumber = "01556473252";
                //user.Location = new Location { Address = "Assiut };
                //user.gender = Gender.Female;
                //    user.FullName = "Asmaa Ibrahim";
                //    user.Age = 22;
                //    user.Region = region.دير_مواس;
                //    user.Email = "asmaaomarr111@gmail.com";
                //    user.Password = "Asmaa_@123";
                //    user ConfirmPassword = "Asmaa_@123";
                    var user = CreateUser();
                    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);

                    user.PhoneNumber = Input.PhoneNumber;
                user.Location = new Location
                {
                    Address = Input.Address,
                    Latitude = Input.Latitude,
                    Longitude = Input.Longitude
                };
                user.gender = Input.Gender;
                    user.FullName = Input.FullName;
                    user.Age = Input.Age;
                    user.Region = Input.Region;

                    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    var allowedRoles = new[] { "Merchant", "Courier","Admin"};

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
                            // send the courier to the courier extra-info page and ask that page to redirect
                            // to the courier dashboard after completion
                            return RedirectToPage($"/Account/CorierRoleRegisteration", new { id = user.Id, returnUrl = "/Courier/Index" });
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