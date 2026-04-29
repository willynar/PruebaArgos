using POCArgos.Models;

namespace POCArgos.Interfaces;

public interface ICatalogsService
{
    Task<List<OrderStatus>> GetOrderStatusesAsync();
    Task<List<ShippingMethod>> GetShippingMethodsAsync();
}
