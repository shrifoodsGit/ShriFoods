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
        public CartItemModel newcartItemModel { get; set; }



        [BindProperty]
        public ProductModel selectedProductModel { get; set; }

        public string ImageBase64 { get; set; }

        [BindProperty]
        public int ItemQuantity { get; set; } = 1;


        //Constructor
        public ProductsModel(ILogger<ProductModel> logger, FoodsDBContext context)
        {
            _logger = logger;
            _dBContext = context;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var productSelected = await _dBContext.ProductsTb.FirstOrDefaultAsync(e => e.ProductId ==id);

            if(productSelected.ProductQty!=null)
            {
                selectedProductModel = productSelected;
                listProductModel.Add(selectedProductModel);
             
            }
            return Page();
        }


        //Saving Product along with quantity to Cart 
        public async Task<IActionResult> OnPostAsync(int id)
        {        
            string session_userName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
            string session_UserContact = HttpContext.Session.GetString("session_UserContact");
            string session_UserEmail = HttpContext.Session.GetString("session_UserEmail");

            if (session_userName == "Guest"||session_userName ==null)
            {
                ViewData["Message"] = "Please SignUp to Book a Ride..";
                Response.Redirect("/SignIn");
            }
            else
            {
                try
                {
                    var productSelected = await _dBContext.ProductsTb.FirstOrDefaultAsync(e => e.ProductId ==id);
                    // Access the selected value via ItemQuantity
                    var result = ItemQuantity;

                    // Returns true if NO data exists in the table
                    bool isTableEmpty = !_dBContext.CartItemTb.Any();
                    if (isTableEmpty)
                    {
                        newcartItemModel.CartId = 1;
                        Console.WriteLine("Table is empty.");
                    }
                    else
                    {
                        // Finds the max Id number and adds +1 to it 
                        var newCartId = _dBContext.CartItemTb.Max(r => r.CartId);
                        newcartItemModel.CartId = newCartId+1;
                        Console.WriteLine("Table has data.");
                    }


                    int totalproductamount = int.Parse(productSelected.ProductPrice);

                    newcartItemModel.ProductId = id.ToString();
                    newcartItemModel.ProductUniqueId = productSelected.ProductUniqueId;
                    newcartItemModel.ProductQty = result.ToString();
                    newcartItemModel.ProductName = productSelected.ProductName;
                    newcartItemModel.ProductPrice = productSelected.ProductPrice;
                    newcartItemModel.ProductWeight = productSelected.ProductWeight;
                    //newcartItemModel.ProductDescription = productSelected.ProductDescription;
                    //newcartItemModel.ProductImage = productSelected.ProductImage;
          
                    newcartItemModel.UserUniqueId =session_UserUniqueId;
                    newcartItemModel.UserFirstName = session_userName;
                    newcartItemModel.CartTotal = (totalproductamount*result).ToString();




                    //Store shopped products into Cart 
                    _dBContext.CartItemTb.Add(newcartItemModel);
                    listCartItemModel.Add(newcartItemModel);
                    _dBContext.SaveChanges();

                    return RedirectToPage("/Cart/Cart");
                }
                catch (Exception ex)
                {
                    return (IActionResult)ex;

                }
            }
            return Page();
        }
    }
}
