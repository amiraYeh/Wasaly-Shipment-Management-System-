using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wasaly.BLL.ViewModels;
using Wasaly.DAL.Data.Context;
using Wasaly.DAL.Models;


namespace Wasaly.PL.Areas.Identity.Pages.Account
{
    public class MerchantRoleRegisterationModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<WasalyIdentityUser> _userManager;
        private readonly SignInManager<WasalyIdentityUser> _signInManager;

        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
        [BindProperty]
        public MerchantCreateViewModel Merchant { get; set; } = new();

        public MerchantRoleRegisterationModel(
            ApplicationDbContext context,
            UserManager<WasalyIdentityUser> userManager,
            SignInManager<WasalyIdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {


            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("??? ???????? ??? ?????");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return Page();
            }

            string storeName = null;
            string businessType = null;
           

            if (Merchant.StoreName != null)
                storeName = Merchant.StoreName;

            if (Merchant.BusinessType != null)
                businessType = Merchant.BusinessType;

           
            var merchant = new Merchant
            {
                WasalyIdentityUserId = id,
                StoreName = storeName,
                BusinessType = businessType
            };

            await _context.Merchants.AddAsync(merchant);
            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect(ReturnUrl ?? "/");
        }

       
    }
}