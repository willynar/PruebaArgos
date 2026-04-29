using Microsoft.EntityFrameworkCore;
using POCArgos.Interfaces;
using POCArgos.Models;

namespace POCArgos.Logic;

public class Catalogs : ICatalogsService
{
    private readonly ApplicationDbContext _context;

    public Catalogs(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderStatus>> GetOrderStatusesAsync()
    {
        await Task.Delay(2000);
        return await _context.OrderStatuses.ToListAsync();
    }

    public async Task<List<ShippingMethod>> GetShippingMethodsAsync()
    {
        await Task.Delay(2000);
        return await _context.ShippingMethods.ToListAsync();
    }
}
