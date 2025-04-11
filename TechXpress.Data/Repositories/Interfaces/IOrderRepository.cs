using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Entities;

namespace TechXpress.Data.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task UpdateOrderAsync(Order order);
    }

}
