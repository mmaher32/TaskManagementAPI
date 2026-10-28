using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    protected readonly DbSet<T> _dbSet = context.Set<T>();


    public async Task<T?> GetByIdAsync(int id, CancellationToken ct)
    => await _dbSet.FindAsync([id], ct);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct)
    => await _dbSet.AsNoTracking().ToListAsync(ct);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
    => await _dbSet.FirstOrDefaultAsync(predicate, ct);
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
    => await _dbSet.AnyAsync(predicate, ct);

    public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
    => _dbSet.CountAsync(predicate, ct);
    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

}
