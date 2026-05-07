using Microsoft.EntityFrameworkCore;
using POCArgos.Interfaces;
using POCArgos.Domain.Entities;
using POCArgos.Data;

namespace POCArgos.Data.Repositories;

public class CatalogRepository : ICatalogRepository
{
    private readonly ApplicationDbContext _context;

    public CatalogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderStatus>> GetOrderStatusesAsync()
    {
        return await _context.OrderStatuses.ToListAsync();
    }

    public async Task<IEnumerable<ShippingMethod>> GetShippingMethodsAsync()
    {
        return await _context.ShippingMethods.ToListAsync();
    }
}
