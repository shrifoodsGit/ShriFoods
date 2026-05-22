using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriFoods.Model;

namespace ShriFoods.Pages
{
   public class VerifyOTPModel : PageModel
    {
        private readonly FoodDbContext _dbcontext;

        public VerifyOTPModel(FoodDbContext context)
        {
            _dbcontext = context;
        }

        [BindProperty]
        public string OTP { get; set; }

        public IActionResult OnPost()
        {
            string email = TempData["Email"]?.ToString();

            var user = _dbcontext.UserTb
                .FirstOrDefault(x => x.UserEmail == email);

            if (user == null)
            {
                return RedirectToPage("ForgotPassword");
            }

            if (user.ResetOTP != OTP)
            {
                ViewData["Message"] = "Invalid OTP";
                return Page();
            }

            if (user.OTPExpiry < DateTime.Now)
            {
                ViewData["Message"] = "OTP Expired";
                return Page();
            }

            TempData["ResetEmail"] = email;

            return RedirectToPage("ResetPassword");
        }
    }
}