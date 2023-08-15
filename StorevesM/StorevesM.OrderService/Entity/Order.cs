using System.ComponentModel.DataAnnotations;

namespace StorevesM.OrderService.Entity
{
    public class Order
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
