using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using TechXpress.Data.Entities.Enums;

namespace TechXpress.Data.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public string? PaymentIntentId { get; set; }

        [Required(ErrorMessage = "Subtotal is required")]
        [Range(0, 999999.99, ErrorMessage = "Invalid subtotal amount")]
        public decimal Subtotal { get; set; }

        [Required(ErrorMessage = "Tax is required")]
        [Range(0, 999999.99, ErrorMessage = "Invalid tax amount")]
        public decimal Tax { get; set; }

        [Required(ErrorMessage = "Shipping fee is required")]
        [Range(0, 999999.99, ErrorMessage = "Invalid shipping fee")]
        public decimal ShippingFee { get; set; }

        [Required(ErrorMessage = "Total is required")]
        [Range(0, 999999.99, ErrorMessage = "Invalid total amount")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Shipping address is required")]
        public  string ShippingAddress { get; set; } = null!;

        [Required(ErrorMessage = "Billing address is required")]
        public string BillingAddress { get; set; } = null!; 
        public string? Notes { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

    }
}
