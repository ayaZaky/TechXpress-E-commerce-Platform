using Microsoft.AspNetCore.Mvc;  
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories.Interfaces;
using TechXpress.Services.Interfaces;
using TechXpress_E_commerce_Platform.View_Models;

namespace TechXpress_E_commerce_Platform.Controllers
{
    // Controllers/CheckoutController.cs
    public class CheckoutController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderRepository _orderRepository;

        public CheckoutController(IPaymentService paymentService, IOrderRepository orderRepository)
        {
            _paymentService = paymentService;
            _orderRepository = orderRepository;
        }

        public ActionResult Index()
        {
            var order = new Order
            {
                TotalAmount = 100.00m  
            };
            return View(order);
        }
         
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            if (ModelState.IsValid)
            {   
                var paymentIntent = await _paymentService.CreatePaymentIntentAsync(order.TotalAmount, "usd");
                   
                order.PaymentIntentId = paymentIntent.Id;
                var createdOrder = await _orderRepository.CreateOrderAsync(order);
                  
                return View(new { ClientSecret = paymentIntent.ClientSecret });
            }

            return View(order);
        }
         
        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(string paymentIntentId)
        {
            var paymentIntent = await _paymentService.ConfirmPaymentAsync(paymentIntentId);
            if (paymentIntent.Status == "succeeded")
            {
                return View("Success"); 
            }                                                                      

            return View("Failure"); 
        }

    }
}
