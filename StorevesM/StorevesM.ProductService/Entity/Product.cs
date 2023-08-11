using System.ComponentModel.DataAnnotations;

namespace StorevesM.ProductService.Entity
{
    public class Product
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Describe { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
