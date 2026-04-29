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
        private readonly FoodsDBContext _dbContext;

        [BindProperty]
        public int newProductId { get; set; }

        public List<ProductModel> list_ProductModel = new List<ProductModel>();
        private DateTime lastStartDate;

        [BindProperty]
        public ProductModel NewProductModel { get; set; }

        // Maps to VARBINARY(MAX) in SQL Server
        [Column(TypeName = "varbinary(max)")]
        public byte[] ImageData { get; set; }



        //Radom Number Generation
        private static readonly Random _random = new Random();
        public int UniqueNumber { get; set; }

        //Constructor 
        public UploadProductsModel(FoodsDBContext context)
        {
            _dbContext = context;
        }


        public void OnGet()
        {
            //string session_userName = HttpContext.Session.GetString("session_UserName");
            //string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
        }

        public async Task<IActionResult> OnPost(string selectedProductImage)
        {
            //string session_userName = HttpContext.Session.GetString("session_UserName");
            //string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");

            UniqueNumber = _random.Next(10000, 100000);
            //Add user session UniqueId into Ride table to know whats he added 
            NewProductModel.ProductUniqueId =  UniqueNumber.ToString();

            // Finds the max Id number and adds +1 to it 
            var newProductId = _dbContext.ProductsTb.Max(r => r.ProductId);


            //Image storinging       
            byte[] imageBytes = System.IO.File.ReadAllBytes(selectedProductImage);

            //using (var ms = new MemoryStream())           
            //{

            //    await uploadingFIle.copy
            //    imageData.Save(ms, imageData.RawFormat);
            //    byte[] imageBytes = ms.ToArray();

            //}

            NewProductModel.ProductImage=imageBytes;

            // Finds the max Id number and adds +1 to it 
            NewProductModel.ProductId = newProductId+1;

            _dbContext.ProductsTb.Add(NewProductModel);

            _dbContext.SaveChanges();

            return RedirectToPage("/AdDashboard");
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
