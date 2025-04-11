using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Stripe;
using TechXpress.Services.Interfaces;

namespace TechXpress.Services.Implementation
{
    public class StripePaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly string _stripeSecretKey;

        public StripePaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
            _stripeSecretKey = _configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }  
        public async Task<PaymentIntent> CreatePaymentIntentAsync(decimal amount, string currency)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100), // Convert to cents
                    Currency = currency.ToLower(),
                    PaymentMethodTypes = new List<string> { "card" },
                    CaptureMethod = "automatic"
                };

                var service = new PaymentIntentService();
                return await service.CreateAsync(options);
            }
            catch (StripeException ex)
            {
                // Log the error
                throw new ApplicationException($"Error creating payment intent: {ex.Message}", ex);
            }
        }

        public async Task<PaymentIntent> ConfirmPaymentAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                return await service.ConfirmAsync(paymentIntentId);
            }
            catch (StripeException ex)
            {
                // Log the error
                throw new ApplicationException($"Error confirming payment: {ex.Message}", ex);
            }
        }

        public async Task<PaymentIntent> CancelPaymentIntentAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                return await service.CancelAsync(paymentIntentId);
            }
            catch (StripeException ex)
            {
                // Log the error
                throw new ApplicationException($"Error canceling payment intent: {ex.Message}", ex);
            }
        }

        public async Task<PaymentIntent> UpdatePaymentIntentAsync(string paymentIntentId, PaymentIntentUpdateOptions options)
        {
            try
            {
                var service = new PaymentIntentService();
                return await service.UpdateAsync(paymentIntentId, options);
            }
            catch (StripeException ex)
            {
                // Log the error
                throw new ApplicationException($"Error updating payment intent: {ex.Message}", ex);
            }
        }
    }
}
