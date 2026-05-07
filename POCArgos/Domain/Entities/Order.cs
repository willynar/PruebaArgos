using Microsoft.EntityFrameworkCore.Migrations;
using System.ComponentModel.DataAnnotations;

namespace POCArgos.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string CustomerName { get; set; } = null!;

    public int OrderStatusId { get; set; }
    public OrderStatus? OrderStatus { get; set; }

    public int ShippingMethodId { get; set; }

    public string? DeliveryAddress { get; set; }

    public string? InternalComent { get; set; }

    public ShippingMethod? ShippingMethod { get; set; }

    public decimal TotalAmount { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}
