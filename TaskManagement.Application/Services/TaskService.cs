using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Application.Services;

public sealed class TaskService(ITaskRepository tasks, IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<TaskResponse>> ListAsync(int userId, CancellationToken ct)
    {
        var items = await tasks.ListOwnedAsync(userId, ct);
        return items.Select(TaskResponse.From).ToList();
    }

    public async Task<Result<TaskResponse>> CreateAsync(int userId, CreateTaskRequest request, CancellationToken ct)
    {
        var result = TaskItem.Create(request.Title, request.Description, userId,
            request.Priority, request.DueDate?.UtcDateTime);
        if (result.IsFailure)
            return Result<TaskResponse>.Failure(result.Error!);

        tasks.Add(result.Value);
        await unitOfWork.SaveChangesAsync(ct);
        return Result<TaskResponse>.Success(TaskResponse.From(result.Value));
    }

    public async Task<Result<TaskResponse>> UpdateAsync(int userId, int id, UpdateTaskRequest request, CancellationToken ct)
    {
        var task = await tasks.FindOwnedAsync(userId, id, ct);
        if (task is null)
            return Result<TaskResponse>.Failure(TaskErrors.NotFound(id));

        var title = task.ChangeTitle(request.Title);
        if (title.IsFailure)
            return Result<TaskResponse>.Failure(title.Error!);

        var priority = task.ChangePriority(request.Priority);
        if (priority.IsFailure)
            return Result<TaskResponse>.Failure(priority.Error!);

        task.ChangeDescription(request.Description);
        task.ChangeDueDate(request.DueDate?.UtcDateTime);
        if (request.IsCompleted)
            task.Complete();
        else
            task.Reopen();

        await unitOfWork.SaveChangesAsync(ct);
        return Result<TaskResponse>.Success(TaskResponse.From(task));
    }

    public async Task<Result> DeleteAsync(int userId, int id, CancellationToken ct)
    {
        var task = await tasks.FindOwnedAsync(userId, id, ct);
        if (task is null)
            return Result.Failure(TaskErrors.NotFound(id));

        tasks.Delete(task);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
