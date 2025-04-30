using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Entities;
using TechXpress.Data.DTOs;

namespace TechXpress.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {


        //Task<Order> CreateOrderAsync(string userId, OrderCompletionRequest request);
        Task Update(Order order);
        Task UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null);
        Task PaymentStatus(int orderId, string SessionId, string PaymentIntentId);
    }

}
