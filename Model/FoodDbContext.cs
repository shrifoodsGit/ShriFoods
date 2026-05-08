using Microsoft.EntityFrameworkCore;

namespace ShriFoods.Model
{
    public class FoodDbContext:DbContext
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options)
        : base(options)
        {
        }

        /*------Old Code--------------*/
        public DbSet<ProductModel> ProductsTb { get; set; }
        public DbSet<UserModel> UserTb { get; set; }
        public DbSet<CartItemModel> CartItemTb { get; set; }
        public DbSet<OrderModel> OrderTb { get; set; }


        public DbSet<NewCartModel> Cart { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        //public DbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<NewCartModel>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId);

        }
    }
}
