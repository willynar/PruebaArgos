using POCArgos.Interfaces;
using POCArgos.Domain.Entities;
using POCArgos.DTOs;

namespace POCArgos.Services;

public class Orders : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public Orders(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderDto>> GetOrdersAsync()
    {
        var orders = await _orderRepository.GetProjectedAsync(OrderDto.Projection);
        return orders.ToList();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id)
    {
        var orders = await _orderRepository.GetProjectedAsync(
            OrderDto.Projection, 
            o => o.Id == id
        );
        return orders.FirstOrDefault();
    }

    public async Task<OrderDto> CreateOrderAsync(OrderUpsertDto dto)
    {
        var order = new Order
        {
            CustomerName = dto.CustomerName,
            OrderStatusId = dto.OrderStatusId,
            ShippingMethodId = dto.ShippingMethodId,
            DeliveryAddress = dto.DeliveryAddress,
            TotalAmount = dto.TotalAmount,
            InternalComent = dto.InternalComent
        };

        var created = await _orderRepository.AddAsync(order);
        return MapToDto(created);
    }

    public async Task<bool> UpdateOrderAsync(int id, OrderUpsertDto dto)
    {
        if (id != dto.Id)
            throw new ArgumentException("Id mismatch");

        var order = new Order
        {
            Id = dto.Id,
            CustomerName = dto.CustomerName,
            OrderStatusId = dto.OrderStatusId,
            ShippingMethodId = dto.ShippingMethodId,
            DeliveryAddress = dto.DeliveryAddress,
            TotalAmount = dto.TotalAmount,
            InternalComent = dto.InternalComent,
            RowVersion = dto.RowVersion
        };

        try
        {
            await _orderRepository.UpdateAsync(order);
            return true;
        }
        catch (Exception)
        {
            if (!await _orderRepository.ExistsAsync(id))
            {
                return false;
            }
            throw;
        }
    }

    private static OrderDto MapToDto(Order o) => new OrderDto
    {
        Id = o.Id,
        CustomerName = o.CustomerName,
        OrderStatusId = o.OrderStatusId,
        OrderStatusName = o.OrderStatus?.Name,
        ShippingMethodId = o.ShippingMethodId,
        ShippingMethodName = o.ShippingMethod?.Name,
        DeliveryAddress = o.DeliveryAddress,
        TotalAmount = o.TotalAmount,
        RowVersion = o.RowVersion
    };
}
