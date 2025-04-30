using System.ComponentModel.DataAnnotations;
using TechXpress.Data.Entities;

namespace TechXpress_E_commerce_Platform.View_Models
{
    public class CheckoutViewModel
    {
        public IEnumerable<CartItem> CartItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total => SubTotal + ShippingCost;
        public string StripePublishableKey { get; set; }
    }

    public class AddressViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Street address is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Invalid postal code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
    }

    public class PaymentIntentCreateRequest
    {
        public decimal ShippingCost { get; set; }
    }

    public class OrderCompletionRequest
    {
        public string PaymentIntentId { get; set; }
        public AddressViewModel BillingAddress { get; set; }
        public AddressViewModel ShippingAddress { get; set; }
       // public decimal ShippingCost { get; set; }
        public string ShippingMethod { get; set; }
    }
}