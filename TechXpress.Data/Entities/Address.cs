using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace  TechXpress.Data.Entities
{
      public class Address
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string UserId { get; set; }
            [Required(ErrorMessage = "Address Type is required")]
            [Display(Name = "Address Type")]
            public string AddressType { get; set; }= null!; // "billing" or "shipping"

            [Required(ErrorMessage = "Street is required")]
            [StringLength(50, ErrorMessage = "Street cannot exceed 50 characters")]
            public string Street { get; set; }=null!;

            [Required(ErrorMessage = "City is required")]
            [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
            public string City { get; set; } = null!;

            [Required(ErrorMessage = "State is required")]
            [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
            public string State { get; set; } = null!;

            [Required(ErrorMessage = "Postal code is required")]
            [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Invalid postal code format")]
            public string PostalCode { get; set; } = null!;

            [Required(ErrorMessage = "Country is required")]
            [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
            public string Country { get; set; } = null!;
            public virtual ApplicationUser User { get; set; } = null!;

        
    }
}
