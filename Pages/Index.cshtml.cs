using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriFoods.Model;

namespace ShriFoods.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly FoodsDBContext _dBContext;
        public List<ProductModel> listProductModel = new List<ProductModel>();

        public List<CartItemModel> listCartItemModel = new List<CartItemModel>();

        [BindProperty]
        public CartItemModel cartItemModel { get; set; }


        public string ImageBase64 { get; set; }

        [BindProperty]
        public int ItemQuantity { get; set; } = 1;
        public IndexModel(FoodsDBContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _dBContext = context; 
        }
        public void OnGet()
        {
            //Creating a session variable 
            string userValue = HttpContext.Session.GetString("session_UserName");

            listProductModel = _dBContext.ProductsTb.ToList();

            //var record = await listProductModel;
            foreach (Model.ProductModel lProducts in listProductModel)
            {
                // Convert byte array to Base64 string
                ImageBase64 = Convert.ToBase64String(lProducts.ProductImage);
            }
        }
    }
}
