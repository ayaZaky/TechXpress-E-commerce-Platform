using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using TechXpress.Data.Entities.Enums;

namespace TechXpress.Data.Entities
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

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual PaymentTransaction PaymentTransaction { get; set; } = null!;
    }
}