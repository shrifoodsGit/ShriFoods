using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriFoods.Model;

namespace ShriFoods.Pages.Customer
{
    public class CustProfileModel : PageModel
    {
        private readonly FoodDbContext _dbContext;
        public List<UserModel> listUserModel = new List<UserModel>();
        public List<UserModel> activeUser = new List<UserModel>();

        public List<NewOrder> list_NewOrderModel = new List<NewOrder>();
        public List<NewOrder> only_NewOrderModel = new List<NewOrder>();

        [BindProperty]
        public CartItemModel updateRecord { get; set; }

        //Constructor
        public CustProfileModel(FoodDbContext context)
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

            //orderItems list display   
            list_NewOrderModel = _dbContext.Orders.ToList();
            foreach (var orderIems in list_NewOrderModel)
            {

                if (orderIems.UserFirstName ==session_UserName)
                {
                    only_NewOrderModel = list_NewOrderModel.FindAll(a => a.UserFirstName == session_UserName);
                    //Major Milestone in achiving only wanted list out of selected index
                    //only_DriverRides.Add(list_SortedRideModel[rideIndex]);
                }

            }
        }

        public IActionResult OnPostContshopping()
        {
            return RedirectToPage("/Index");
        }
    }
}
