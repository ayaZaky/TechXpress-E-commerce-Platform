namespace TechXpress_E_commerce_Platform.View_Models
{
    public class PaymentIntentDto
    {
        public string Id { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentMethodId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string ReceiptEmail { get; set; }
    }
}
