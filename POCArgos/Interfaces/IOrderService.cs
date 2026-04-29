using POCArgos.Models;

namespace POCArgos.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(Order order);
    Task<bool> UpdateOrderAsync(int id, Order order);
}
