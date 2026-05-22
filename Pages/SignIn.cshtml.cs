using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriFoods.Model;
using ShriFoods.Pages.Helpers;

namespace ShriFoods.Pages
{
    public class SignInModel : PageModel
    {
        private readonly FoodDbContext _dbContext;


        private RedirectToPageResult returnpage;
        public List<UserModel> listUserModel = new List<UserModel>();

        public SignInModel(FoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
        }
      
        
        public IActionResult OnPost(string InputEmail, string InputPswd)
        {
            var passwordHelper = new PasswordHelper();
            //listUserModel = _dbContext.UserTb.ToList();
            var user = _dbContext.UserTb.FirstOrDefault(x => x.UserEmail == InputEmail);
            bool loginSuccess = false;
            if (user!=null)
            {
                if (passwordHelper.IsHashed(user.UserPswd))
                {
                     loginSuccess = passwordHelper.VerifyPassword(
                        user.UserPswd,
                        InputPswd
                    );
                }
                else
                {
                    // OLD PLAIN TEXT USERS

                    if (user.UserPswd == InputPswd)
                    {
                        loginSuccess = true;

                        // AUTO CONVERT TO HASH

                        user.UserPswd =
                            passwordHelper.HashPassword(InputPswd);

                        _dbContext.SaveChanges();
                    }
                }

                if (loginSuccess)
                    {
                    // Login success

                    // Clears the session data if it holds any
                    HttpContext.Session.Clear();

                    //Session Start, Creating a session variables 
                    HttpContext.Session.SetInt32("session_UserId", user.UserId);
                    HttpContext.Session.SetString("session_UserName", user.UserFirstName);
                    HttpContext.Session.SetString("session_UserUniqueId", user.UserUniqueId);
                    HttpContext.Session.SetString("session_UserContact", user.UserContact);
                    HttpContext.Session.SetString("session_UserEmail", user.UserEmail);
                    HttpContext.Session.SetString("session_UserRole", user.UserRole);

                    if (user.UserRole=="Cust")
                    {
                        //Signin Validated
                        returnpage= RedirectToPage("/Customer/CustProfile");
                    }
                    else if (user.UserRole=="Admin")
                    {
                        returnpage= RedirectToPage("/Admin/AdDashboard");
                    }
                    return returnpage;
                }
                else
                {
                      ViewData["Message"] = "Enter the write Password";
                      return Page();
             
                }
     
            }
            else
            {
                ViewData["Message"] = " Email ID doesn't exist--Please Sign Up";
                return Page();
                //return RedirectToPage("/SignUp");

            }
           
        }
    }
}
