namespace TaskManagement.Application.DTOs;

public record CreateTaskRequest(string Title, string? Description = null,
    int Priority = 1, DateTimeOffset? DueDate = null);

public record UpdateTaskRequest(string Title, bool IsCompleted, int Priority = 1,
    string? Description = null, DateTimeOffset? DueDate = null);
