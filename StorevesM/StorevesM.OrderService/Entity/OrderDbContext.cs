using Microsoft.EntityFrameworkCore;

namespace StorevesM.OrderService.Entity
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> opt) : base(opt)
        {
        }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    }
}
