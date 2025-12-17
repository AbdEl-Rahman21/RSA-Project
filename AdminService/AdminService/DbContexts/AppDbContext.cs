using AdminService.Models;
using System.Data.Entity;

namespace AdminService.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base(@"Server=(localdb)\MSSQLLocalDB;Initial Catalog=RSA;Integrated Security=True")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<FAQ> FAQ { get; set; }
        public DbSet<Notification> Notification { get; set; }
    }
}
