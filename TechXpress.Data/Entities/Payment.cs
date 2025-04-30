using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using TechXpress.Data.Entities.Enums;

namespace  TechXpress.Data.Entities
{
    public class Payment
    { 
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = null!; // Credit Card, PayPal, etc.

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public  PaymentStatus Status { get; set; } = PaymentStatus.Pending; // Pending, Completed, Failed

        public string? TransactionId { get; set; }                                                                                  

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public virtual Order Order { get; set; } = null!;
       
    }
        
   
}
