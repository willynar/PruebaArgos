using POCArgos.Domain.Entities;

namespace POCArgos.Interfaces;

/// <summary>
/// Interfaz específica para operaciones de Pedidos. 
/// Permite extender funcionalidades que no están en el repositorio genérico.
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    // Aquí podrías añadir métodos específicos como Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
}
