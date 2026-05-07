using POCArgos.Interfaces;
using POCArgos.Domain.Entities;
using POCArgos.Data;

namespace POCArgos.Data.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Aquí implementarías métodos específicos si los hubiera en IOrderRepository
}
