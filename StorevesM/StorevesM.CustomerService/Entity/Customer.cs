using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace StorevesM.CustomerService.Entity
{
    public class Customer
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Username { get; set; }

        [Required]
        [MaxLength(250)]
        public string Password { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
