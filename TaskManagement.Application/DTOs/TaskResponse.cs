using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.DTOs;

public record TaskResponse(int Id, string Title, string Description, int Priority,
    bool IsCompleted, DateTime? DueDate, DateTime CreatedAt)
{
    public static TaskResponse From(TaskItem task) =>
        new(task.Id, task.Title, task.Description, task.Priority,
            task.IsCompleted, task.DueDate, task.CreatedAt);
}
