using POCArgos.Domain.Entities;
using POCArgos.DTOs;

namespace POCArgos.Interfaces;

public interface IOrderService
{
    Task<List<OrderDto>> GetOrdersAsync();
    Task<OrderDto?> GetOrderByIdAsync(int id);
    Task<OrderDto> CreateOrderAsync(OrderUpsertDto orderDto);
    Task<bool> UpdateOrderAsync(int id, OrderUpsertDto orderDto);
}
