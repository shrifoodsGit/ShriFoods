using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriFoods.Model;

namespace ShriFoods.Pages.Admin
{
    public class AdDashboardModel : PageModel
    {

        public List<UserModel> list_UserModel = new List<UserModel>();
        public List<ProductModel> list_ProductModel = new List<ProductModel>();
        public List<NewOrder> list_OrderModel = new List<NewOrder>();


        private readonly FoodDbContext _dbContext;
        //Constructor 
        public AdDashboardModel(FoodDbContext context)
        {
            _dbContext = context;
        }
        public void OnGet()
        {
            string session_UserName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");


            if (session_UserName =="ShriPavan" && session_UserUniqueId =="54863")
            {
                //User List display table 
                list_UserModel = _dbContext.UserTb.ToList();

                //Arrange list as per date and time 
                list_ProductModel = _dbContext.ProductsTb.ToList();
                //listRideModel = _dbContext.RideDBTable.ToList();
                //return RedirectToPage("/Admin/AdminDashboard");

                //Orders Table 
                list_OrderModel = _dbContext.Orders.ToList();
            }
            else
            {
                RedirectToPage("./SignIn");
            }

        }

        //If in future you admin wants to delete old orders/orders....dont delete untill unless needed to 
        public async Task<IActionResult> OnPostDeleteAsync(int orderId)
        {
            // Get order details first
            var orderDetails = await _dbContext.OrderDetails
                .Where(x => x.OrderId == orderId)
                .ToListAsync();

            // Delete child rows
            _dbContext.OrderDetails.RemoveRange(orderDetails);

            // Get main order
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(x => x.OrderId == orderId);

            if (order != null)
            {
                _dbContext.Orders.Remove(order);
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/AdminOrders");
        }
    }
}
