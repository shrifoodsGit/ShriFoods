
using Microsoft.EntityFrameworkCore;
namespace ShriFoods.Model
{
    public class FoodsDBContext:DbContext
    {
        public FoodsDBContext(DbContextOptions<FoodsDBContext> options):base(options)
            { }

        public DbSet<ProductModel> ProductsTb { get; set; }
        public DbSet<UserModel> UserTb { get; set; }
        public DbSet<CartItemModel> CartItemTb { get; set; }
        public DbSet<OrdersModel> OrderTb { get; set; }

    }
}
