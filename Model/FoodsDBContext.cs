
using Microsoft.EntityFrameworkCore;
namespace ShriFoods.Model
{
    public class FoodsDBContext:DbContext
    {
        public FoodsDBContext(DbContextOptions<FoodsDBContext> options):base(options)
            { }

        public DbSet<ProductModel> ProductsTb { get; set; }

    }
}
