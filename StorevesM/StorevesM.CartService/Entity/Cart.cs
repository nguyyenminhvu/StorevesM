using System.ComponentModel.DataAnnotations;

namespace StorevesM.CartService.Entity
{
    public class Cart
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
