using System.Linq.Expressions;

namespace POCArgos.Interfaces;

/// <summary>
/// Interfaz genérica que define las operaciones básicas de datos (CRUD).
/// Sigue el principio de Responsabilidad Única (SRP) al separar el acceso a datos.
/// </summary>
/// <typeparam name="T">La entidad con la que se trabajará.</typeparam>
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<TResult>> GetProjectedAsync<TResult>(
        Expression<Func<T, TResult>> projection,
        Expression<Func<T, bool>>? predicate = null);
}
