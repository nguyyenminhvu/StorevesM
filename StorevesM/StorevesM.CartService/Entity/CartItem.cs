using System.ComponentModel.DataAnnotations;

namespace StorevesM.CartService.Entity
{
    public class CartItem
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }


        public Cart Cart { get; set; } 
    }
}
