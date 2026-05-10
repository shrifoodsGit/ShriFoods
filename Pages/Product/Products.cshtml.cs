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
        //private readonly FoodsDBContext _dBContext;
        private readonly FoodDbContext _context;
        public List<ProductModel> listProductModel = new List<ProductModel>();

        public List<CartItemModel> listCartItemModel = new List<CartItemModel>();

        [BindProperty]
        public CartItemModel newcartItemModel { get; set; }

        [BindProperty]
        public NewCartModel lCart { get; set; }

        [BindProperty]
        public ProductModel selectedProductModel { get; set; }

        public string ImageBase64 { get; set; }

        [BindProperty]
        public int ItemQuantity { get; set; } = 1;


        //Constructor
        public ProductsModel(ILogger<ProductModel> logger, FoodDbContext newcontext)
        {
            _logger = logger;
            _context = newcontext;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var productSelected = await _context.ProductsTb.FirstOrDefaultAsync(e => e.ProductId ==id);

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
            if (HttpContext.Session==null)
            {
                Response.Redirect("/SignIn");
            }
            else
            {
               
                string session_userName = HttpContext.Session.GetString("session_UserName");
                string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
                string session_UserContact = HttpContext.Session.GetString("session_UserContact");
                string session_UserEmail = HttpContext.Session.GetString("session_UserEmail");

                if (session_userName == "Guest"||session_userName ==null)
                {
                    ViewData["Message"] = "Please SignIn/SignUp to purchase selected product(s).....";
                    Response.Redirect("/SignIn");
                }
                else
                {
                    int session_UserId = (int)HttpContext.Session.GetInt32("session_UserId");
                    try
                    {
                        var productSelected = await _context.ProductsTb.FirstOrDefaultAsync(e => e.ProductId ==id);

                        if (productSelected == null)
                            throw new Exception("Product not found");

                        var existingCart = await _context.Cart
                            .FirstOrDefaultAsync(x => x.UserId == session_UserId.ToString() && x.ProductId == id);
                        bool isTableEmpty = !_context.Cart.Any();
                        if (isTableEmpty)
                        {
                            NewCartModel cart = new NewCartModel
                            {
                                UserId = session_UserId.ToString(),
                                ProductId = id,
                                Quantity = ItemQuantity,
                                Price = productSelected.ProductPrice,
                                AddedDate = DateTime.Now,
                                ProductName = productSelected.ProductName,
                                Product =productSelected
                            };
                            _context.Cart.Add(cart);
                        }
                        else
                        {
                            if (existingCart != null)
                            {
                                existingCart.Quantity += 1;
                            }
                            else
                            {
                                NewCartModel cart = new NewCartModel
                                {
                                    UserId = session_UserId.ToString(),
                                    ProductId = id,
                                    Quantity = ItemQuantity,
                                    Price = productSelected.ProductPrice,
                                    AddedDate = DateTime.Now,
                                    ProductName = productSelected.ProductName,
                                    Product =productSelected
                                };

                                _context.Cart.Add(cart);
                            }


                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error at Add To cart:"+ex.ToString());
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Cart/Cart");
                }
   
            }
            return Page();
        }

        public async Task<IActionResult> AddToCart(int userId, int productId, int quantity)
        {
            //var product = await _context.ProductsTb.FirstOrDefaultAsync(x=>x.ProductId ==productId);
            
            return Page();
        }
    }
}
