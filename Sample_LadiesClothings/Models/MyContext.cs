using Microsoft.EntityFrameworkCore;


namespace Sample_LadiesClothings.Models
{
    public class MyContext:DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }
        public DbSet<Admin> tbl_admin { get; set; }
        public DbSet<Customer> tbl_customer { get; set; }
        public DbSet<Category> tbl_category { get; set; }
        public DbSet<Product> tbl_products { get; set; }
        public DbSet<Cart> tbl_cart { get; set; }
        public DbSet<Feedback> tbl_feedback { get; set; }
        public DbSet<FAQ> tbl_faq { get; set; }
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("tbl_customer");
            modelBuilder.Entity<Category>().ToTable("tbl_category");
            modelBuilder.Entity<Product>().ToTable("tbl_products");
            modelBuilder.Entity<Order>().ToTable("tbl_orders");
            modelBuilder.Entity<OrderItem>().ToTable("tbl_order_items");
            modelBuilder.Entity<Customer>().HasKey(x => x.Customer_Id);
            modelBuilder.Entity<Category>().HasKey(x => x.Category_Id);
            modelBuilder.Entity<Order>().HasKey(x => x.Order_Id);
            modelBuilder.Entity<OrderItem>().HasKey(x => x.OrderItem_Id);


            modelBuilder.Entity<Product>().HasOne(p => p.category)
                .WithMany(c => c.products)
                .HasForeignKey(p => p.Category_Id);

            // Constraints
            modelBuilder.Entity<Customer>().HasIndex(c => c.Customer_Email).IsUnique();

            modelBuilder.Entity<OrderItem>()
               .HasOne<Order>()
               .WithMany(o => o.Items)
               .HasForeignKey(oi => oi.Order_Id)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
