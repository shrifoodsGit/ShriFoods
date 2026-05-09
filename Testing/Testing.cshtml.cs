using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xunit;

namespace ShriFoods.Testing
{
    public class TestingModel : PageModel
    {
        public void OnGet()
        {
        }

        [Fact]
        public void OnPostOrders()
        {
            RedirectToPage("/Orders");
        }
    }
}
