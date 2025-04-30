using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechXpress.Data.AppContext;
using TechXpress.Data.DTOs;
using TechXpress.Data.Entities;
using TechXpress.Data.Entities.Enums;
using TechXpress.Data.Repositories.Interfaces;
using TechXpress_E_commerce.Repositories;

namespace TechXpress.Data.Repositories.Implementation
{
    public class OrderRepository : Repository<Order> , IOrderRepository
    {
        private readonly AppDbContext _context;    

        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Update method
        public async Task Update(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        // UpdateStatus method
        public async Task UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order != null)
            {
                if (Enum.TryParse(orderStatus, true, out OrderStatus parsedOrderStatus))
                {
                    order.OrderStatus = parsedOrderStatus;
                }

                if (!string.IsNullOrEmpty(paymentStatus) && Enum.TryParse(paymentStatus, true, out PaymentStatus parsedPaymentStatus))
                {
                    order.PaymentStatus = parsedPaymentStatus;
                }

                await _context.SaveChangesAsync();
            }
        }

        // PaymentStatus method
        public async Task PaymentStatus(int orderId, string sessionId, string paymentIntentId)
        {

            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order != null)
            {
                order.PaymentDate = DateTime.Now;
                order.PaymentIntentId = paymentIntentId;
                order.SessionId = sessionId;
                await _context.SaveChangesAsync();
            }
        }
        //public void PaymentStatus(int orderId, string SessionId, string PaymentIntentId)
        //{
        //    var  order = _context.Orders.FirstOrDefault(x => x.Id == orderId);
        //    order.DateOfPayment = DateTime.Now;
        //    order.PaymentIntentId = PaymentIntentId;
        //    order.SessionId = SessionId;
        //}


    }
}
