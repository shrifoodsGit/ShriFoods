using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriFoods.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ShriFoods.Pages.Admin
{
    public class UploadProductsModel : PageModel
    {
        private readonly FoodDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public int newProductId { get; set; }

        public List<ProductModel> list_ProductModel = new List<ProductModel>();
        private DateTime lastStartDate;

        [BindProperty]
        public ProductModel NewProductModel { get; set; }


        [BindProperty]
        public IFormFile ImageFile { get; set; }

        //Radom Number Generation
        private static readonly Random _random = new Random();
        public int UniqueNumber { get; set; }

        //Constructor 
        public UploadProductsModel(FoodDbContext context, IWebHostEnvironment environment)
        {
            _dbContext = context;
            _environment=environment;
        }


        public void OnGet()
        {
            //string session_userName = HttpContext.Session.GetString("session_UserName");
            //string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
        }

        public async Task<IActionResult> OnPostAsync(string selectedProductImage)
        {
            //string session_userName = HttpContext.Session.GetString("session_UserName");
            //string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");

            UniqueNumber = _random.Next(10000, 100000);
            //Add user session UniqueId into Ride table to know whats he added 
            NewProductModel.ProductUniqueId =  UniqueNumber.ToString();

            // Finds the max Id number and adds +1 to it 
            var newProductId = _dbContext.ProductsTb.Max(r => r.ProductId);

            // Finds the max Id number and adds +1 to it 
            NewProductModel.ProductId = newProductId+1;


            //ImagePath Upload
            if (ImageFile != null)
            {
                // Create uploads folder path
                string uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "img/Products");

                //// Create folder if not exists
                //if (!Directory.Exists(uploadsFolder))
                //{
                //    Directory.CreateDirectory(uploadsFolder);
                //}

                // Unique filename
                string uniqueFileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(ImageFile.FileName);

                // Full file path
                string filePath = Path.Combine(
                    uploadsFolder,
                    uniqueFileName);

                // Save image
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                // Save image path in DB
                NewProductModel.ProductImagePath =
                    "/img/Products/" + uniqueFileName;
            }


            _dbContext.ProductsTb.Add(NewProductModel);

            _dbContext.SaveChanges();

            return RedirectToPage("/Admin/AdDashboard");
        }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime LastStartDate
        {
            get { return lastStartDate; }
            set { lastStartDate = value; }
        }
    }
}
