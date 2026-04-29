using Microsoft.EntityFrameworkCore;
using POCArgos.Interfaces;
using POCArgos.Models;

namespace POCArgos.Logic;

public class Orders : IOrderService
{
    private readonly ApplicationDbContext _context;

    public Orders(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingMethod)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingMethod)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<bool> UpdateOrderAsync(int id, Order order)
    {
        if (id != order.Id)
            throw new ArgumentException("Id mismatch");

        _context.Entry(order).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await OrderExistsAsync(id))
            {
                return false;
            }
            else
            {
                throw;
            }
        }
    }

    private async Task<bool> OrderExistsAsync(int id)
    {
        return await _context.Orders.AnyAsync(e => e.Id == id);
    }
}
