using Microsoft.EntityFrameworkCore;
using OrderService.Model;

namespace OrderService
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }

        public DbSet<Orders> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orders>().HasKey(t=> new { t.OrderID });
           
        }
    }
}
