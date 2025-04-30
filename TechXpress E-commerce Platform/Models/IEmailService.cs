namespace TechXpress_E_commerce_Platform.Models
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string body);
    }

}
