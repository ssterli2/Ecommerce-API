using Microsoft.EntityFrameworkCore;
 
namespace Ecomm.Models
{
    public class EcommContext : DbContext
    {
        public EcommContext(DbContextOptions<EcommContext> options) : base(options) { }

        public DbSet<Product> product { get; set;} 
        public DbSet<Cart> cart { get; set;} 
        public DbSet<CartItem> cartItem { get; set;} 
        public DbSet<Order> order { get; set;} 
        public DbSet<OrderItem> orderItem { get; set;} 
    }
}