using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using TechXpress_E_commerce.Models.Enums;

namespace TechXpress_E_commerce.Models
{
    public class Refund
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Payment transaction ID is required")]
        public int PaymentTransactionId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Invalid refund amount")]
        public decimal Amount { get; set; }

        [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
        public string? Reason { get; set; }

        public PaymentStatus Status { get; set; }

        public string? ProviderRefundId { get; set; } 

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual PaymentTransaction PaymentTransaction { get; set; } = null!;
    }
}