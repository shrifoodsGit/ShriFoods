using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriFoods.Model;

namespace ShriFoods.Pages.Product
{
    public class ProductsModel : PageModel
    {
        private readonly ILogger<ProductModel> _logger;
        private readonly FoodsDBContext _dBContext;
        public List<ProductModel> listProductModel = new List<ProductModel>();

        //Constructor
        public ProductsModel(ILogger<ProductModel> logger, FoodsDBContext context)
        {
            _logger = logger;
            _dBContext = context;
        }
        public void OnGet()
        {
            listProductModel = _dBContext.ProductsTb.ToList();
        }
    }
}
