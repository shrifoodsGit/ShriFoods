using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriFoods.Model;

namespace ShriFoods.Pages
{
    public class SignInModel : PageModel
    {
        private readonly FoodsDBContext _dbContext;


        private RedirectToPageResult returnpage;
        public List<UserModel> listUserModel = new List<UserModel>();

        public SignInModel(FoodsDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string InputEmail, string InputPswd)
        {
            listUserModel = _dbContext.UserTb.ToList();
            foreach (var user in listUserModel)
            {
                if (InputEmail==user.UserEmail && InputPswd == user.UserPswd)
                {

                    // Clears the session data if it holds any
                    HttpContext.Session.Clear();

                    //Session Start, Creating a session variables 
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
            }
            return RedirectToPage();
        }
    }
}
