using System.ComponentModel.DataAnnotations;


namespace TechXpress_E_commerce_Platform.View_Models 
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]

        public string? Email { get; set; }
       // [Required]
        //public string? ClientUrl { get; set; }
    }
}
