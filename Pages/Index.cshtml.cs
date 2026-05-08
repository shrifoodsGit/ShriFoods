using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriFoods.Model;

namespace ShriFoods.Pages
{
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
        public void OnGet()
        {
            //Creating a session variable 
            string userValue = HttpContext.Session.GetString("session_UserName");

            listProductModel = _dBContext.ProductsTb.ToList();



            //Pilling Images into list 
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
