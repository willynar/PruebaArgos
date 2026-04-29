using System.ComponentModel.DataAnnotations;

namespace POCArgos.Models;

public class Order
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string CustomerName { get; set; } = null!;

    public int OrderStatusId { get; set; }
    public OrderStatus? OrderStatus { get; set; }

    public int ShippingMethodId { get; set; }
    public ShippingMethod? ShippingMethod { get; set; }

    public decimal TotalAmount { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}
