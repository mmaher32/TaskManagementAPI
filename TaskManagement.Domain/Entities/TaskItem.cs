using TaskManagement.Domain.Common;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Domain.Entities;

public class TaskItem
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int Priority { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime? DueDate { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public int UserId { get; private set; }
    public User User { get; set; } = null!;
    private TaskItem() { }
    public static Result<TaskItem> Create(
        string title,
        string? description,
        int userId,
        int priority = 1,
        DateTime? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result<TaskItem>.Failure(TaskErrors.EmptyTitle);

        if (userId <= 0)
            return Result<TaskItem>.Failure(TaskErrors.InvalidUserId);

        if (priority is < 1 or > 3)
            return Result<TaskItem>.Failure(TaskErrors.InvalidPriority);

        var task = new TaskItem
        {
            Title = title.Trim(),
            Description = description?.Trim() ?? string.Empty,
            UserId = userId,
            Priority = priority,
            DueDate = dueDate,
            CreatedAt = DateTime.UtcNow,
            IsCompleted = false
        };

        return Result<TaskItem>.Success(task);
    }

    public Result ChangeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure(TaskErrors.EmptyTitle);

        Title = title.Trim();
        return Result.Success();
    }

    public Result ChangePriority(int priority)
    {
        if (priority is < 1 or > 3)
            return Result.Failure(TaskErrors.InvalidPriority);

        Priority = priority;
        return Result.Success();
    }

    public void ChangeDescription(string? description)
    {
        Description = description?.Trim() ?? string.Empty;
    }

    public void ChangeDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
    }

    public void Complete()
    {
        IsCompleted = true;
    }

    public void Reopen()
    {
        IsCompleted = false;
    }
}