using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriFoods.Model;
using ShriFoods.Pages.Helpers;

namespace ShriFoods.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly FoodDbContext _dbcontext;

        public ResetPasswordModel(FoodDbContext context)
        {
            _dbcontext = context;
        }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public IActionResult OnPost()
        {
            string email = TempData["ResetEmail"]?.ToString();

            var user = _dbcontext.UserTb
                .FirstOrDefault(x => x.UserEmail == email);

            if (user == null)
            {
                return RedirectToPage("ForgotPassword");
            }

            PasswordHelper passwordHelper = new PasswordHelper();


            if (NewPassword != ConfirmPassword)
            {
                ViewData["Message"] =
                    "Passwords do not match";

                return Page();
            }
            if (NewPassword.Length < 8)
            {
                ViewData["Message"] =
                    "Password must be minimum 8 characters";

                return Page();
            }
            user.UserPswd =
                passwordHelper.HashPassword(NewPassword);

            user.ResetOTP = null;
            user.OTPExpiry = null;

            _dbcontext.SaveChanges();

            TempData["Message"] =
                "Password reset successful";

            return RedirectToPage("SignIn");
        }
    }
}