using POCArgos.Domain.Entities;

namespace POCArgos.Interfaces;

/// <summary>
/// Interfaz para acceso a catálogos (Estados y Métodos de envío).
/// </summary>
public interface ICatalogRepository
{
    Task<IEnumerable<OrderStatus>> GetOrderStatusesAsync();
    Task<IEnumerable<ShippingMethod>> GetShippingMethodsAsync();
}
