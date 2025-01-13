﻿using System.ComponentModel.DataAnnotations;

namespace TechXpress_E_commerce.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }  
        public DateTime CreatedAt { get; set; } 

        // Navigation properties
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
