using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;     

namespace TechXpress.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreatePaymentIntentAsync(decimal amount, string currency);
        Task<PaymentIntent> ConfirmPaymentAsync(string paymentIntentId);
        Task<PaymentIntent> CancelPaymentIntentAsync(string paymentIntentId);
        Task<PaymentIntent> UpdatePaymentIntentAsync(string paymentIntentId, PaymentIntentUpdateOptions options);
    }
}
