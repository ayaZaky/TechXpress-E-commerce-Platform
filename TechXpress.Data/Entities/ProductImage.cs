using System.ComponentModel.DataAnnotations;

namespace  TechXpress.Data.Entities
{
    public class ProductImage
    { 
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        public string ImageUrl { get; set; } = null!;

        // Navigation property to the Product
        public virtual Product Product { get; set; } = null!;
         
    }
}
