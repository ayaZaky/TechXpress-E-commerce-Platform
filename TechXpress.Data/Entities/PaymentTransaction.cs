using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using TechXpress.Data.Entities.Enums;

namespace  TechXpress.Data.Entities
{
    public class PaymentTransaction
    {
        [Key]
        public int Id { get; set; }  

        [Required(ErrorMessage = "Order ID is required")]
        public int OrderId { get; set; }  

        public int? PaymentMethodId { get; set; }  

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Invalid amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be 3 characters")]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Invalid currency format")]
        public string Currency { get; set; } = null!; 
        public PaymentStatus Status { get; set; } 
        public string? ProviderTransactionId { get; set; }
        public string? ProviderIntentId { get; set; }
        public string? ErrorMessage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Order Order { get; set; } = null!;
        public virtual PaymentMethod PaymentMethod { get; set; } = null!;

        public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();
    }
}
