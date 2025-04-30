using System.ComponentModel.DataAnnotations;

namespace TechXpress.Data.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Order ID is required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Invalid unit price")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Subtotal is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Invalid subtotal")]
        public decimal Subtotal { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;

    }
}
