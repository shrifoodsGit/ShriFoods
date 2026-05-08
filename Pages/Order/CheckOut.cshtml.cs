using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriFoods.Model;
using System.Net;
using System.Numerics;
using Twilio.Types;

namespace ShriFoods.Pages.Order
{
    public class CheckOutModel : PageModel
    {
        private readonly FoodDbContext _dbContext;
        public List<UserModel> listUserModel = new List<UserModel>();
        public List<UserModel> activeUser = new List<UserModel>();


        public List<NewCartModel> list_NewCartModel = new List<NewCartModel>();
        public List<NewCartModel> only_NewCartModel = new List<NewCartModel>();

        public List<OrdersModel> list_OrdersModel = new List<OrdersModel>();
        public List<OrdersModel> only_OrdersModel = new List<OrdersModel>();

        private static readonly Random _random = new Random();
        public int UniqueNumber { get; set; }

        public List<string> grandTotal = new List<string>();

        [BindProperty]
        public int totL { get; set; }

        [BindProperty]
        public int bind_UserId { get; set; }


        [BindProperty]
        public OrderModel newOrderModel { get; set; }

        //Constructor
        public CheckOutModel(FoodDbContext context)
        {
            _dbContext = context;
        }

        public void OnGet()
        {

            //return await _.Orders
            //    .Include(x => x.OrderDetails)
            //    .ThenInclude(x => x.Product)
            //    .Where(x => x.UserId==userId)
            //    .OrderByDescending(x => x.OrderedDate)
            //    .ToListAsync();


            //Display All selected items 
            int session_UserId = (int)HttpContext.Session.GetInt32("session_UserId");
            string session_UserName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
            string session_UserRole = HttpContext.Session.GetString("session_UserRole");


            //cartItems list display   
            if (_dbContext.Cart.ToList()!=null)
            {
                list_NewCartModel = _dbContext.Cart.ToList();
                foreach (var cartItems in list_NewCartModel)
                {
                    if (cartItems.UserId ==session_UserId.ToString())
                    {
                        //int total = (cartItems.CartTotal).Sum(n=>int.Parse(n));
                        //grandTotal.Add(cartItems.);
                        bind_UserId = int.Parse(cartItems.UserId);
                        //int totL = (int.Parse(cartItems.CartTotal));
                        only_NewCartModel = list_NewCartModel.FindAll(a => a.UserId == session_UserId.ToString());

                    }
                }
                //Grand Total to display 
                //totL = grandTotal.Sum(n => int.Parse(n));
            }
        }

        public async Task<IActionResult> OnPostOrder(string id)
        {
            //Placing order, so store it in Order table 


            var userItems = await _dbContext.UserTb
                .FirstOrDefaultAsync(x => x.UserId==(int.Parse(id)));

            var cartItems = await _dbContext.Cart
                .Include(x => x.Product).Where(x => x.UserId==id).ToListAsync();

            if (!cartItems.Any())
                throw new Exception("Cart is empty"); 
            decimal totalAmount=cartItems.Sum(x => x.Quantity*x.Price);

            NewOrder order =new NewOrder
            { 
                UserId=id,
                OrderNumber="ORD-"+DateTime.Now.Ticks,
                TotalAmount=totalAmount,
                OrderStatus="Pending",
                PaymentStatus="Pending",
                ShippingAddress=userItems.UserAddress,
                PhoneNumber=userItems.UserContact,
                OrderedDate=DateTime.Now
            }; 
            
            _dbContext.Orders.Add(order); 
            await _dbContext.SaveChangesAsync(); 
            
            foreach (var item in cartItems) 
            { 
                OrderDetail detail=new OrderDetail
                    { 
                    OrderId=order.OrderId,
                    ProductId=item.ProductId,
                    Quantity=item.Quantity,
                    UnitPrice=item.Price,
 
                    };
                _dbContext.OrderDetails.Add(detail); 
            }
           // _dbContext.Cart.RemoveRange(cartItems); 
            
            await _dbContext.SaveChangesAsync(); 
            
            //return order.OrderId;


            //Display All selected items 
            //int session_UserId = (int)HttpContext.Session.GetInt32("session_UserId");
            //string session_UserName = HttpContext.Session.GetString("session_UserName");
            //string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
            //string session_UserRole = HttpContext.Session.GetString("session_UserRole");

            //// Check weather table is empty or not ?, Returns true if NO data exists in the table
            //bool isTableEmpty = !_dbContext.OrderTb.Any();
            //if (isTableEmpty)
            //{
            //    newOrderModel.OrderId = 1;
            //    Console.WriteLine("Table is empty.");
            //}
            //else
            //{
            //    // Finds the max Id number and adds +1 to it 
            //    var newOrderId = _dbContext.OrderTb.Max(r => r.OrderId);
            //    newOrderModel.OrderId  = newOrderId+1;
            //    Console.WriteLine("Table has data.");
            //}

            //// Generate a random number between 1,000,000 and 9,999,999
            //UniqueNumber = _random.Next(10000, 100000);
            //newOrderModel.OrderUniqueId = UniqueNumber.ToString();

            ////CartItems list display   
            //if (_dbContext.CartItemTb.ToList()!=null)
            //{
            //    list_NewCartModel = _dbContext.Cart.ToList();
            //    foreach (var cartItems in list_NewCartModel)
            //    {
            //        if (cartItems.UserId ==session_UserUniqueId)
            //        {
            //            grandTotal.Add(cartItems.CartTotal);

            //            newOrderModel.CartId=cartItems.CartId;
            //            newOrderModel.CartTotal = cartItems.CartTotal;
            //            newOrderModel.CustomerName=cartItems.UserFirstName;
            //            newOrderModel.CustomerUniqueid=cartItems.UserUniqueId;
            //            newOrderModel.ProductName = cartItems.ProductName;
            //            newOrderModel.ProductPrice=cartItems.ProductPrice;
            //            newOrderModel.ProductQty=cartItems.ProductQty;

            //            _dbContext.OrderTb.Add(newOrderModel);
            //            //int totL = (int.Parse(cartItems.CartTotal));
            //            only_NewCartModel = list_NewCartModel.FindAll(a => a.UserId == session_UserId.ToString());
            //        }
            //    }  

            //}

            ////add user
            //listUserModel = _dbContext.UserTb.ToList();
            //foreach (var user in listUserModel)
            //{
            //    int index = listUserModel.FindIndex(a => a.UserFirstName == session_UserName);
            //    if (user.UserFirstName ==session_UserName)
            //    {
            //        newOrderModel.CustomerContact =user.UserContact;
            //        newOrderModel.CustomerEMail =user.UserEmail;
            //        newOrderModel.CustomerAddress =user.UserAddress;
            //        _dbContext.OrderTb.Add(newOrderModel);
            //    }

            //}

            //newOrderModel.OrderDate = DateOnly.FromDateTime(DateTime.Now);

            ////Add all the Order info into orders Table 
            //totL = grandTotal.Sum(n => int.Parse(n));
            //newOrderModel.GrandTotal = totL.ToString();    




            ////Add and Save to DB

            //_dbContext.SaveChanges();

            return RedirectToPage("/Order/Orders");
        }
    }
}