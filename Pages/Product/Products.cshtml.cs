using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriFoods.Model;

namespace ShriFoods.Pages.Product
{
    public class ProductsModel : PageModel
    {
        private readonly ILogger<ProductModel> _logger;
        private readonly FoodsDBContext _dBContext;
        public List<ProductModel> listProductModel = new List<ProductModel>();
        public string ImageBase64 { get; set; }
        //Constructor
        public ProductsModel(ILogger<ProductModel> logger, FoodsDBContext context)
        {
            _logger = logger;
            _dBContext = context;
        }
        public void OnGet()
        {
            listProductModel = _dBContext.ProductsTb.ToList();

            //var record = await listProductModel;
            foreach(Model.ProductModel lProducts in listProductModel)
            {
                // Convert byte array to Base64 string
                ImageBase64 = Convert.ToBase64String(lProducts.ProductImage);
            }
        }
    }
}
