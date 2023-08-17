using Microsoft.EntityFrameworkCore;

namespace StorevesM.CustomerService.Entity
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> opt) : base(opt)
        {

        }
        public DbSet<Customer> Customers => Set<Customer>();
    }
}
