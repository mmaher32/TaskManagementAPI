using System.Linq.Expressions;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);

    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
