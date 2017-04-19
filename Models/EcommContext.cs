using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
 
namespace Ecomm.Models
{
    public class EcommContext : IdentityDbContext
    {
        public EcommContext(DbContextOptions<EcommContext> options) : base(options) { }

        public DbSet<User> user { get; set; }
        public DbSet<Product> product { get; set;} 
        public DbSet<Order> order { get; set;} 
    }
}