using Microsoft.EntityFrameworkCore;

namespace StorevesM.CartService.Entity
{
    public class CartServiceDbcontext : DbContext
    {
        public CartServiceDbcontext(DbContextOptions<CartServiceDbcontext> opt) : base(opt)
        {

        }
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
    }
}
