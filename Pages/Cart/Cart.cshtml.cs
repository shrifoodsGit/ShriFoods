using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriFoods.Model;

namespace ShriFoods.Pages.Cart
{
    public class CartModel : PageModel
    {
        private readonly FoodsDBContext _dbContext;
        public List<UserModel> listUserModel = new List<UserModel>();
        public List<UserModel> activeUser = new List<UserModel>();

        public List<CartItemModel> list_CartItemModel = new List<CartItemModel>();
        public List<CartItemModel> only_CartItemModel = new List<CartItemModel>();

        [BindProperty]
        public CartItemModel updateRecord { get; set; }

        //Constructor
        public CartModel(FoodsDBContext context)
        {
            _dbContext = context;
        }


        public void OnGet()
        {
            string session_UserName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
            string session_UserRole = HttpContext.Session.GetString("session_UserRole");
            //User profile display
            listUserModel = _dbContext.UserTb.ToList();
            foreach (var user in listUserModel)
            {
                int index = listUserModel.FindIndex(a => a.UserFirstName == session_UserName);
                if (user.UserFirstName ==session_UserName)
                {
                    //Major Milestone in achiving only wanted list out of selected index
                    activeUser.Add(listUserModel[index]);
                }

            }

            //cartItems list display   
            list_CartItemModel = _dbContext.CartItemTb.ToList();
            foreach (var cartItems in list_CartItemModel)
            {

                if (cartItems.ProductUniqueId ==session_UserUniqueId)
                {
                    only_CartItemModel = list_CartItemModel.FindAll(a => a.ProductUniqueId == session_UserUniqueId);
                    //Major Milestone in achiving only wanted list out of selected index
                    //only_DriverRides.Add(list_SortedRideModel[rideIndex]);
                }

            }
        }
    }
}
