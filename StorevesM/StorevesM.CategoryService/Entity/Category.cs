﻿using System.ComponentModel.DataAnnotations;

namespace StorevesM.CategoryService.Entity
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
