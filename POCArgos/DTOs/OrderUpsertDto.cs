using System.ComponentModel.DataAnnotations;

namespace POCArgos.DTOs;

public class OrderUpsertDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
    [MaxLength(100)]
    public string CustomerName { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado válido")]
    public int OrderStatusId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un método de envío válido")]
    public int ShippingMethodId { get; set; }

    public string? DeliveryAddress { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El monto total no puede ser negativo")]
    public decimal TotalAmount { get; set; }

    public string? InternalComent { get; set; }

    public byte[]? RowVersion { get; set; }
}
