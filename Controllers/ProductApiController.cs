using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using ShriFoods.Data;
using ShriFoods.Model;

namespace ShriFoods.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        private readonly FoodDbContext _context;

        public ProductApiController(FoodDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.ProductsTb.ToListAsync();


            foreach(var product in products)
            {
                product.ProductImagePath =
"https://shrifoods-dgb4dhbbhpeud7gd.canadacentral-01.azurewebsites.net"
+ product.ProductImagePath;
            }

            
            return Ok(products);
        }
    }
}