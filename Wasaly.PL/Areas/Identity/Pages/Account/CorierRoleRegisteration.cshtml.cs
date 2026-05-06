using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wasaly.BLL.ViewModels;
using Wasaly.DAL.Data.Context;
using Wasaly.DAL.Models;

namespace Wasaly.PL.Areas.Identity.Pages.Account
{
    public class CorierRoleRegisterationModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<WasalyIdentityUser> _userManager;
        private readonly SignInManager<WasalyIdentityUser> _signInManager;

        //public string ReturnUrl { get; set; }
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        //public string UserId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
        [BindProperty]
        public CourierViewModel Courier { get; set; } = new();

        public CorierRoleRegisterationModel(
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

            string nationalIdPath = null;
            string drivingLicensePath = null;
            string profileImagePath = null;

            if (Courier.NationalIdImage != null)
                nationalIdPath = await SaveFileAsync(Courier.NationalIdImage);

            if (Courier.DrivingLicenseImage != null)
                drivingLicensePath = await SaveFileAsync(Courier.DrivingLicenseImage);

            if (Courier.ProfileImage != null)
                profileImagePath = await SaveFileAsync(Courier.ProfileImage);

            var courier = new Courier
            {
                WasalyIdentityUserId = id,
                NationalIdImagePath = nationalIdPath,
                DrivingLicenseImagePath = drivingLicensePath,
                ProfileImagePath = profileImagePath
            };

            await _context.Couriers.AddAsync(courier);
            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect(ReturnUrl ?? "/");
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "couriers");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/couriers/" + uniqueFileName;
        }
    }
}