using AdminService.Models;
using System.Data.Entity;

namespace AdminService.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection") { }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartProduct> Cart_Products { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProduct> Order_Products { get; set; }
        public DbSet<FAQ> FAQ { get; set; }
    }
}
