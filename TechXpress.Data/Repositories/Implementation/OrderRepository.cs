using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechXpress.Data.AppContext;
using TechXpress.Data.Entities;
using TechXpress.Data.Repositories.Interfaces;

namespace TechXpress.Data.Repositories.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(AppDbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Create an order
        public async Task<Order> CreateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Order with ID {order.Id} created successfully.");
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the order.");
                throw new Exception("An error occurred while creating the order.", ex);
            }
        }

        // Get order by ID
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {orderId} not found.");
                    return null;
                }
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving the order.");
                throw new Exception("An error occurred while retrieving the order.", ex);
            }
        }

        // Get orders for a specific user
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .ToListAsync();

                if (!orders.Any())
                {
                    _logger.LogWarning($"No orders found for user with ID {userId}.");
                }

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving orders for user with ID {userId}.");
                throw new Exception("An error occurred while retrieving user orders.", ex);
            }
        }

        // Update an existing order
        public async Task UpdateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Order with ID {order.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the order.");
                throw new Exception("An error occurred while updating the order.", ex);
            }
        }
    }

}
