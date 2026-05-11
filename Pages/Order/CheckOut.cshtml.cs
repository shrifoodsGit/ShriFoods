using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShriFoods.Model;
using System.Net;
using System.Numerics;
using Twilio.Types;
using Xunit;

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
        public int bind_OrderId { get; set; }


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
                UserEmail = userItems.UserEmail,
                UserFirstName = userItems.UserFirstName,
                OrderNumber="ORD-"+DateTime.Now.Ticks,
                TotalAmount=totalAmount,
                OrderStatus="Pending",
                PaymentStatus="Pending",
                PaymentMethod ="COD",
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
            bind_OrderId = order.OrderId;

            //Delete from cart onceorder placed successfully 
            _dbContext.Cart.RemoveRange(cartItems); 

            await _dbContext.SaveChangesAsync(); 
            
            return RedirectToPage("../Order/Orders", new { id = bind_OrderId });
        }

        //Testing
        [Fact]
        public void OnPost_checkout()
        {
            RedirectToPage("/Orders");
            //// 1. Arrange
            //var pageModel = new (); // Or Controller

            //// 2. Act
            //var result = pageModel.OnPost("");

            //// 3. Assert
            //// Verify it is a RedirectToPageResult
            //var redirectResult = Assert.IsType<RedirectToPageResult>(result);

            //// Verify the destination page name
            //Assert.Equal("Index", redirectResult.PageName);
        }

        [Fact]
        public IActionResult OnPostTest()
        {
            // Your logic here...
            return RedirectToPage("Details");

            // Or using a relative path explicitly
            // return RedirectToPage("./Details"); 
        }


    }

    
    }