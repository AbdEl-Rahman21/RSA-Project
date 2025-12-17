using AdminService.Models;
using System.Data.Entity;

namespace AdminService.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection") { }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<FAQ> FAQ { get; set; }
    }
}
