using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriFoods.Model;
using Microsoft.AspNetCore.OutputCaching;

namespace ShriFoods.Pages
{
    [OutputCache(Duration = 60)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        //private readonly FoodsDBContext _dBContext;
        private readonly FoodDbContext _dBContext;
        private readonly IWebHostEnvironment _env;

        public List<ProductModel> listProductModel = new List<ProductModel>();

        public List<CartItemModel> listCartItemModel = new List<CartItemModel>();

        public List<string> AttaImageUrls { get; set; } = new List<string>();

        [BindProperty]
        public CartItemModel cartItemModel { get; set; }

        //[BindProperty]
        //public string ImageBase64 { get; set; }

        [BindProperty]
        public int ItemQuantity { get; set; } = 1;
        public IndexModel(FoodDbContext context, ILogger<IndexModel> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _dBContext = context;
            _env = env;
        }
        public async Task OnGetAsync()
        {
            //Creating a session variable 
            string userValue = HttpContext.Session.GetString("session_UserName");

          

            var products = await _dBContext.ProductsTb.
                Select(p => new ProductModel
                {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        ProductPrice = p.ProductPrice,
                        ProductImagePath = p.ProductImagePath
                    })
                    .ToListAsync();
            listProductModel = products;

            //Filling Images into list 
            string folderName = "img/Products/Atta/MultiGrain";
            string path = Path.Combine(_env.WebRootPath, folderName);

            if (Directory.Exists(path))
            {
                //Get all Image files
                var files = Directory.GetFiles(path, "*.*")
                .Where(s => s.EndsWith(".jpg") ||s.EndsWith(".png")||s.EndsWith(".gif"));

                foreach (var file in files)
                {
                    //Convert physical path to a web-relative URL
                    AttaImageUrls.Add($"/{folderName}/{Path.GetFileName(file)}");
                }
            }
        }
    }
}
