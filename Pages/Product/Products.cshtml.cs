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
            int session_UserId = (int)HttpContext.Session.GetInt32("session_UserId");
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
                            Price = decimal.Parse(productSelected.ProductPrice),
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
                                Price = decimal.Parse(productSelected.ProductPrice),
                                AddedDate = DateTime.Now,
                                ProductName = productSelected.ProductName,
                                Product =productSelected
                            };

                            _context.Cart.Add(cart);
                        }

                
                    }
               
                    //if (existingCart != null)
                    //{
                    //    existingCart.Quantity += quantity;
                    //}
                    //else
                    //{


                    //    NewCartModel cart = new NewCartModel
                    //    {
                    //        UserId = userId,
                    //        ProductId = productId,
                    //        Quantity = quantity,
                    //        Price = decimal.Parse(productSelected.ProductPrice),
                    //        AddedDate = DateTime.Now
                    //    };

                    //    _context.Cart.Add(cart);
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error at Add To cart:"+ex.ToString());
                }

                 await _context.SaveChangesAsync();

                //try
                //{

                //    AddToCart(session_UserId, id, ItemQuantity);

                //    var productSelected = await _context.ProductsTb.FirstOrDefaultAsync(e => e.ProductId ==id);
                //    // Access the selected value via ItemQuantity
                //    var result = ItemQuantity;

                //    // Returns true if NO data exists in the table
                //    bool isTableEmpty = !_context.CartItemTb.Any();
                //    if (isTableEmpty)
                //    {
                //        newcartItemModel.CartId = 1;
                //        Console.WriteLine("Table is empty.");
                //    }
                //    else
                //    {
                //        // Finds the max Id number and adds +1 to it 
                //        var newCartId = _context.CartItemTb.Max(r => r.CartId);
                //        newcartItemModel.CartId = newCartId+1;
                //        Console.WriteLine("Table has data.");
                //    }


                //    int totalproductamount = int.Parse(productSelected.ProductPrice);

                //    newcartItemModel.ProductId = id.ToString();
                //    newcartItemModel.ProductUniqueId = productSelected.ProductUniqueId;
                //    newcartItemModel.ProductQty = result.ToString();
                //    newcartItemModel.ProductName = productSelected.ProductName;
                //    newcartItemModel.ProductPrice = productSelected.ProductPrice;
                //    newcartItemModel.ProductWeight = productSelected.ProductWeight;
                //    //newcartItemModel.ProductDescription = productSelected.ProductDescription;
                //    //newcartItemModel.ProductImage = productSelected.ProductImage;

                //    newcartItemModel.UserUniqueId =session_UserUniqueId;
                //    newcartItemModel.UserFirstName = session_userName;
                //    newcartItemModel.CartTotal = (totalproductamount*result).ToString();




                //    //Store shopped products into Cart 
                //    _context.CartItemTb.Add(newcartItemModel);
                //    listCartItemModel.Add(newcartItemModel);
                //    _context.SaveChanges();

                //    return RedirectToPage("/Cart/Cart");
                //}
                //catch (Exception ex)
                //{
                //    return (IActionResult)ex;

                //}
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
