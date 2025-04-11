using System.ComponentModel.DataAnnotations;

namespace TechXpress_E_commerce_Platform.View_Models
{
    public class CheckoutViewModel
    {
        // Personal Details
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        // Billing Address
        [Required(ErrorMessage = "Mailing address is required")]
        [Display(Name = "Mailing Address")]
        public string MailingAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Post code is required")]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "State is required")]
        [Display(Name = "State/Region")]
        public string State { get; set; }

        public bool SameAsShipping { get; set; }

        // Shipping Address
        public string ShippingFirstName { get; set; }
        public string ShippingLastName { get; set; }
        public string ShippingEmail { get; set; }
        public string ShippingPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingPostCode { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingState { get; set; }

        // Shipping Method
        [Required(ErrorMessage = "Please select a shipping method")]
        public string ShippingMethod { get; set; }

        // Payment Information
        [Required(ErrorMessage = "Cardholder name is required")]
        [Display(Name = "Cardholder Name")]
        public string CardholderName { get; set; }

        [Required(ErrorMessage = "Card number is required")]
        [CreditCard(ErrorMessage = "Invalid card number")]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiry month is required")]
        [Range(1, 12, ErrorMessage = "Invalid month")]
        public int ExpiryMonth { get; set; }

        [Required(ErrorMessage = "Expiry year is required")]
        public int ExpiryYear { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "Invalid CVV")]
        public string CVV { get; set; }

        // Order Summary
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        public string CouponCode { get; set; }
    }
}