using System.ComponentModel.DataAnnotations;

namespace TechXpress_E_commerce_Platform.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {

        public int Id { get; set; } 
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 200 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [StringLength(100)]
        public string? Brand { get; set; }

        [Range(1, 5)]
        public double Rating { get; set; }

        public string? Specifications { get; set; }

        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }


        // صور من خلال رفع من الجهاز
        public List<IFormFile>? UploadedImages { get; set; }       


        // صور من خلال روابط
        public List<string>? ImageUrls { get; set; }
    }
}
