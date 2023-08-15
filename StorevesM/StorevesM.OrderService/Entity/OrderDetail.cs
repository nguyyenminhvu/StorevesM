using System.ComponentModel.DataAnnotations;

namespace StorevesM.OrderService.Entity
{
    public class OrderDetail
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }

        public Order Order { get; set; }
    }
}
