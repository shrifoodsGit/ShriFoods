using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriFoods.Model;
using System.ComponentModel.DataAnnotations;

namespace ShriFoods.Pages.Cart
{
    public class CartModel : PageModel
    {
        private readonly FoodDbContext _dbContext;
        public List<UserModel> listUserModel = new List<UserModel>();
        public List<UserModel> activeUser = new List<UserModel>();

        public List<NewCartModel> list_CartModel = new List<NewCartModel>();
        public List<NewCartModel> only_CartModel = new List<NewCartModel>();


        //Constructor
        public CartModel(FoodDbContext context)
        {
            _dbContext = context;
        }



        public void OnGet()
        {
            //Display All selected items 
            int session_UserId = (int)HttpContext.Session.GetInt32("session_UserId");
            string session_UserName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
            string session_UserRole = HttpContext.Session.GetString("session_UserRole");
            ////User profile display
            //listUserModel = _dbContext.UserTb.ToList();
            //foreach (var user in listUserModel)
            //{
            //    int index = listUserModel.FindIndex(a => a.UserFirstName == session_UserName);
            //    if (user.UserFirstName ==session_UserName)
            //    {
            //        //Major Milestone in achiving only wanted list out of selected index
            //        activeUser.Add(listUserModel[index]);
            //    }

            //}

           //ToDo-Make sure to handle empty/null cart its throwing error

            //cartItems list display   
            if (_dbContext.Cart.ToList()!=null) { 
            list_CartModel = _dbContext.Cart.ToList();
            foreach (var cartItems in list_CartModel)
            {

                if (cartItems.UserId ==session_UserId.ToString())
                {
                    only_CartModel = list_CartModel.FindAll(a => a.UserId == session_UserId.ToString());
                    //Major Milestone in achiving only wanted list out of selected index
                    //only_DriverRides.Add(list_SortedRideModel[rideIndex]);
                }

            }
            }
        }


        public async Task<List<NewCartModel>> GetCartItems(string userId)
        {
            return await _dbContext.Cart.Include(x => x.Product).Where(x => x.UserId==userId).ToListAsync();
        }

        //To Delete selected product from cart
        public async Task OnPostDelete(int cartId)
        {
            var item=await _dbContext.Cart.FindAsync(cartId); 
            if (item!=null) 
            {
                _dbContext.Cart.Remove(item); 
                await _dbContext.SaveChangesAsync(); 
            }
        }

    }
}
