using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriFoods.Model;
using ShriFoods.Pages.Services;
using System.Net;
using System.Net.Mail;

namespace ShriFoods.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly FoodDbContext _dbcontext;
        private readonly EmailService _emailService;

        public ForgotPasswordModel(FoodDbContext context, EmailService emailService)
        {
            _dbcontext = context;
            _emailService=emailService;
        }

        [BindProperty]
        public string UserEmail { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _dbcontext.UserTb
                .FirstOrDefault(x => x.UserEmail == UserEmail);

            if (user == null)
            {
                ViewData["Message"] = "Email not found";
                return Page();
            }

            Random random = new Random();

            string otp = random.Next(100000, 999999).ToString();

            user.ResetOTP = otp;
            user.OTPExpiry = DateTime.Now.AddMinutes(10);

            _dbcontext.SaveChanges();

            await _emailService.SendOTPEmail(UserEmail, otp);  

            TempData["Email"] = UserEmail;

            return RedirectToPage("VerifyOTP");
        }

        private void SendOTPEmail(string email, string otp)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("ShriFoods@gmail.com");

            mail.To.Add(email);

            mail.Subject = "ShriFoods Password Reset OTP";

            mail.Body = $"Your OTP is: {otp}";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");

            smtp.Port = 587;

            smtp.Credentials = new NetworkCredential(
                "yourmail@gmail.com",
                "your-app-password"
            );

            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
    }
}