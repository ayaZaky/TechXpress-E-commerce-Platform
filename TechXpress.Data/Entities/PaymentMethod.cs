using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace TechXpress.Data.Entities
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }  

        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }  

        [Required(ErrorMessage = "Provider is required")]
        [StringLength(50, ErrorMessage = "Provider cannot exceed 50 characters")]
        public string Provider { get; set; } = null!;

        [Required(ErrorMessage = "Payment type is required")]
        [StringLength(50, ErrorMessage = "Payment type cannot exceed 50 characters")]
        public string PaymentType { get; set; } = null!;

        [StringLength(4, ErrorMessage = "Last four digits must be exactly 4 characters")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Last four must be 4 digits")]
        public string? LastFour { get; set; }

        [Range(1, 12, ErrorMessage = "Invalid expiry month")]
        public int? ExpiryMonth { get; set; }

        [Range(2024, 2099, ErrorMessage = "Invalid expiry year")]
        public int? ExpiryYear { get; set; }

        [StringLength(100, ErrorMessage = "Card holder name cannot exceed 100 characters")]
        public string? CardHolderName { get; set; }

        public bool IsDefault { get; set; }

        public string? ProviderPaymentMethodId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set;}
        public virtual ICollection<Refund> Refunds { get; set; }  
    }
}