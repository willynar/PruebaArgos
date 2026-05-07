using System.Linq.Expressions;
using POCArgos.Domain.Entities;

namespace POCArgos.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = null!;
    public int OrderStatusId { get; set; }
    public string? OrderStatusName { get; set; }
    public int ShippingMethodId { get; set; }
    public string? ShippingMethodName { get; set; }
    public string? DeliveryAddress { get; set; }
    public decimal TotalAmount { get; set; }
    public byte[]? RowVersion { get; set; }

    public static Expression<Func<Order, OrderDto>> Projection => o => new OrderDto
    {
        Id = o.Id,
        CustomerName = o.CustomerName,
        OrderStatusId = o.OrderStatusId,
        OrderStatusName = o.OrderStatus != null ? o.OrderStatus.Name : null,
        ShippingMethodId = o.ShippingMethodId,
        ShippingMethodName = o.ShippingMethod != null ? o.ShippingMethod.Name : null,
        DeliveryAddress = o.DeliveryAddress,
        TotalAmount = o.TotalAmount,
        RowVersion = o.RowVersion
    };
}
