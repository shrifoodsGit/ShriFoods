using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriFoods.Model;

namespace ShriFoods.Pages.Order
{
    public class CheckOutModel : PageModel
    {
        private readonly FoodDbContext _dbContext;
        public List<UserModel> listUserModel = new List<UserModel>();
        public List<UserModel> activeUser = new List<UserModel>();

        public List<CartItemModel> list_CartItemModel = new List<CartItemModel>();
        public List<CartItemModel> only_CartItemModel = new List<CartItemModel>();

        public List<OrdersModel> list_OrdersModel = new List<OrdersModel>();
        public List<OrdersModel> only_OrdersModel = new List<OrdersModel>();

        private static readonly Random _random = new Random();
        public int UniqueNumber { get; set; }

        public List<string> grandTotal = new List<string>();

        [BindProperty]
        public int totL { get; set; }

        [BindProperty]
        public OrderModel newOrderModel { get; set; }

        //Constructor
        public CheckOutModel(FoodDbContext context)
        {
            _dbContext = context;
        }

        public void OnGet()
        {
            //Display All selected items 
            string session_UserName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
            string session_UserRole = HttpContext.Session.GetString("session_UserRole");


            //cartItems list display   
            if (_dbContext.CartItemTb.ToList()!=null)
            {
                list_CartItemModel = _dbContext.CartItemTb.ToList();
                foreach (var cartItems in list_CartItemModel)
                {
                    if (cartItems.UserUniqueId ==session_UserUniqueId)
                    {
                        //int total = (cartItems.CartTotal).Sum(n=>int.Parse(n));
                        grandTotal.Add(cartItems.CartTotal);

                        //int totL = (int.Parse(cartItems.CartTotal));
                        only_CartItemModel = list_CartItemModel.FindAll(a => a.UserUniqueId == session_UserUniqueId);

                    }
                }
                //Grand Total to display 
                totL = grandTotal.Sum(n => int.Parse(n));
            }
        }

        public async Task<IActionResult> OnPostOrder(string id)
        {
            //Placing order, so store it in Order table 

            //Display All selected items 
            string session_UserName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
            string session_UserRole = HttpContext.Session.GetString("session_UserRole");

            // Check weather table is empty or not ?, Returns true if NO data exists in the table
            bool isTableEmpty = !_dbContext.OrderTb.Any();
            if (isTableEmpty)
            {
                newOrderModel.OrderId = 1;
                Console.WriteLine("Table is empty.");
            }
            else
            {
                // Finds the max Id number and adds +1 to it 
                var newOrderId = _dbContext.OrderTb.Max(r => r.OrderId);
                newOrderModel.OrderId  = newOrderId+1;
                Console.WriteLine("Table has data.");
            }

            // Generate a random number between 1,000,000 and 9,999,999
            UniqueNumber = _random.Next(10000, 100000);
            newOrderModel.OrderUniqueId = UniqueNumber.ToString();

            //CartItems list display   
            if (_dbContext.CartItemTb.ToList()!=null)
            {
                list_CartItemModel = _dbContext.CartItemTb.ToList();
                foreach (var cartItems in list_CartItemModel)
                {
                    if (cartItems.UserUniqueId ==session_UserUniqueId)
                    {
                        grandTotal.Add(cartItems.CartTotal);

                        newOrderModel.CartId=cartItems.CartId;
                        newOrderModel.CartTotal = cartItems.CartTotal;
                        newOrderModel.CustomerName=cartItems.UserFirstName;
                        newOrderModel.CustomerUniqueid=cartItems.UserUniqueId;
                        newOrderModel.ProductName = cartItems.ProductName;
                        newOrderModel.ProductPrice=cartItems.ProductPrice;
                        newOrderModel.ProductQty=cartItems.ProductQty;

                        _dbContext.OrderTb.Add(newOrderModel);
                        //int totL = (int.Parse(cartItems.CartTotal));
                        only_CartItemModel = list_CartItemModel.FindAll(a => a.UserUniqueId == session_UserUniqueId);
                    }
                }  
                
            }

            //add user
            listUserModel = _dbContext.UserTb.ToList();
            foreach (var user in listUserModel)
            {
                int index = listUserModel.FindIndex(a => a.UserFirstName == session_UserName);
                if (user.UserFirstName ==session_UserName)
                {
                    newOrderModel.CustomerContact =user.UserContact;
                    newOrderModel.CustomerEMail =user.UserEmail;
                    newOrderModel.CustomerAddress =user.UserAddress;
                    _dbContext.OrderTb.Add(newOrderModel);
                }

            }

            newOrderModel.OrderDate = DateOnly.FromDateTime(DateTime.Now);

            //Add all the Order info into orders Table 
            totL = grandTotal.Sum(n => int.Parse(n));
            newOrderModel.GrandTotal = totL.ToString();    
         



            //Add and Save to DB
   
            _dbContext.SaveChanges();

            return RedirectToPage("/Order/Orders",new {id=newOrderModel.OrderUniqueId});
        }
    }
}