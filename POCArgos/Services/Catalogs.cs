using POCArgos.Interfaces;
using POCArgos.Domain.Entities;

namespace POCArgos.Services;

public class Catalogs : ICatalogsService
{
    private readonly ICatalogRepository _catalogRepository;

    public Catalogs(ICatalogRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }

    public async Task<List<OrderStatus>> GetOrderStatusesAsync()
    {
        await Task.Delay(2000); // Simulación de latencia
        var result = await _catalogRepository.GetOrderStatusesAsync();
        return result.ToList();
    }

    public async Task<List<ShippingMethod>> GetShippingMethodsAsync()
    {
        await Task.Delay(2000); // Simulación de latencia
        var result = await _catalogRepository.GetShippingMethodsAsync();
        return result.ToList();
    }
}
