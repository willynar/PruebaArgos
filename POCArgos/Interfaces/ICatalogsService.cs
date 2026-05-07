using POCArgos.Domain.Entities;

namespace POCArgos.Interfaces;

public interface ICatalogsService
{
    Task<List<OrderStatus>> GetOrderStatusesAsync();
    Task<List<ShippingMethod>> GetShippingMethodsAsync();
}
