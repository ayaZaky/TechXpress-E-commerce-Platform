using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;  
using TechXpress.Data.Entities;
using TechXpress.Data.Entities.Enums;
using TechXpress.Data.Repositories.Interfaces;
using TechXpress.Services.Interfaces;
using  TechXpress.Data.DTOs;
using Stripe.Checkout;
using TechXpress.Data.Repositories.Implementation;
using TechXpress.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TechXpress_E_commerce_Platform.Models;
using Microsoft.Extensions.Options;


namespace TechXpress_E_commerce_Platform.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly ICartService _cartService;
        private readonly StripeSettings _stripeSettings;

        public CheckoutController(IUnitOfWork unitofWork, ICartService cartService, IOptions<StripeSettings> stripeSettings )
        {
            _unitofWork = unitofWork;
            _cartService = cartService;
            _stripeSettings = stripeSettings.Value;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cartItems = await _cartService.GetCartItemsAsync(userId);

            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            } 
            var applicationUser = await _unitofWork.User.GetUserWithAddressAsync(userId);
            
            if (applicationUser == null || applicationUser.Address == null)
            {
                return RedirectToAction("Error");   
            }

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                SubTotal = cartItems.Sum(item => item.Product.Price * item.Quantity),
                ShippingCost = 10.50m,
                Order = new Order
                {
                    UserId = userId,
                    Name = applicationUser.FirstName + " " + applicationUser.LastName,
                    StreetAddress = applicationUser.Address.Street,
                    PhoneNumber = applicationUser.PhoneNumber,
                    City = applicationUser.Address.City,
                    PostalCode = applicationUser.Address.PostalCode,
                    State = applicationUser.Address.State,
                    User = applicationUser
                }
            };

            return View(checkoutViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentProcess(CheckoutViewModel vm)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cartItems = await _cartService.GetCartItemsAsync(userId);
            var applicationUser = await _unitofWork.User.GetUserWithAddressAsync(userId);
         
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }   
            var ShippingCost = 10.50m;  
            vm.CartItems = cartItems;
            vm.SubTotal = cartItems.Sum(item => item.Product.Price * item.Quantity);
            vm.Order.OrderTotal = (double)(cartItems.Sum(item => item.Product.Price * item.Quantity) + ShippingCost);
            vm.Order.UserId = userId;
            vm.Order.Name = applicationUser.FirstName + " " + applicationUser.LastName;
            vm.Order.OrderDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));  
            vm.Order.OrderStatus = OrderStatus.Pending;
            vm.Order.PaymentStatus = PaymentStatus.Pending;
            vm.Order.StreetAddress = applicationUser.Address.Street;
            vm.Order.PhoneNumber = applicationUser.PhoneNumber;
            vm.Order.City = applicationUser.Address.City;
            vm.Order.PostalCode = applicationUser.Address.PostalCode;
            vm.Order.State = applicationUser.Address.State;
            vm.Order.User = applicationUser;
            vm.Order.ShippingDate = DateTime.Now.AddDays(7); 
            vm.Order.PaymentDate = DateTime.Now;
            vm.Order.PaymentDueDate = DateTime.Now.AddDays(30);  
         
            await _unitofWork.Order.AddAsync(vm.Order);
            await _unitofWork.CompleteAsync();
         
            var tasks = cartItems.Select(item =>
            {
                var orderDetail = new OrderItem
                {
                    ProductId = item.ProductId,
                    OrderId = vm.Order.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                };
                return _unitofWork.OrderItem.AddAsync(orderDetail);  
            }).ToList();
            
            await Task.WhenAll(tasks);
            await _unitofWork.CompleteAsync();
          
            var domain = "https://localhost:7029";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"/Checkout/OrderSuccess?id={vm.Order.Id}",
                CancelUrl = domain + $"/Cart/Index"
            };
         
            foreach (var item in cartItems)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100), 
                        Currency = "usd", 
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = item.Quantity
                });
            }

            var service = new SessionService();
            Session session = service.Create(options);  
            Session fullSession = service.Get(session.Id);  
        
            if (session == null)
            {    
                return RedirectToAction("Error", "Home");
            }
            Console.WriteLine("Stripe Session ID: " + session.Id);
            Console.WriteLine("Stripe PaymentIntent ID: " + session.PaymentIntentId);
          
            await _unitofWork.Order.PaymentStatus(vm.Order.Id, fullSession.Id, fullSession.PaymentIntentId);    
            await _unitofWork.CompleteAsync();
          
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);  
        }

        public async Task<IActionResult> OrderSuccess(int id)
        {   
            var order = await _unitofWork.Order.GetFirstOrDefaultAsync(x => x.Id == id);
         
            if (order == null)
            {
                return NotFound();  
            }

            var service = new SessionService();
            Session session = service.Get(order.SessionId);   
            if (session == null)
            {
                return RedirectToAction("Error", "Home");  
            } 
            if (session.PaymentStatus?.ToLower() == "paid")
            {
               
                await _unitofWork.Order.UpdateStatus(id, "Shipped", "Completed");
                  
                if (string.IsNullOrEmpty(order.PaymentIntentId) && !string.IsNullOrEmpty(session.PaymentIntentId))
                {
                    order.PaymentIntentId = session.PaymentIntentId;
                    await _unitofWork.CompleteAsync();
                }
            }
            else
            {
                return RedirectToAction("PaymentFailed", "Checkout"); 
            }     
            var carts = await _unitofWork.Cart.FindAsync(x => x.UserId == order.UserId);
            _unitofWork.Cart.RemoveRange(carts);
            await _unitofWork.CompleteAsync();

            return View(id);  
        }


        public IActionResult Cancel()
        {
            return View();
        }   
    }


}
     