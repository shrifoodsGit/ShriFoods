using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriFoods.Model;
using ShriFoods.Pages.Cart;

namespace ShriFoods.Pages.Product
{
    public class ProductsModel : PageModel
    {
        private readonly ILogger<ProductModel> _logger;
        private readonly FoodsDBContext _dBContext;
        public List<ProductModel> listProductModel = new List<ProductModel>();

        public List<CartItemModel> listCartItemModel = new List<CartItemModel>();

        [BindProperty]
        public CartItemModel cartItemModel { get; set; }


        public string ImageBase64 { get; set; }

        [BindProperty]
        public int ItemQuantity { get; set; } = 1;


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

        public async Task<IActionResult> OnPost(string productUniqueId)
        {
            var productselected = await _dBContext.ProductsTb.FirstOrDefaultAsync(e=>e.ProductUniqueId == productUniqueId);

            // Access the selected value via ItemQuantity
            var result = ItemQuantity.ToString();


            cartItemModel.ProductUniqueId = productUniqueId;
            return RedirectToPage("/Cart/Cart");
        }
    }
}
