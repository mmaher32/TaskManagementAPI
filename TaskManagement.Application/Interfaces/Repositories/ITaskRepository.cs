using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> ListOwnedAsync(int userId, CancellationToken ct);
    Task<TaskItem?> FindOwnedAsync(int userId, int id, CancellationToken ct);
    void Add(TaskItem task);
    void Delete(TaskItem task);
}
