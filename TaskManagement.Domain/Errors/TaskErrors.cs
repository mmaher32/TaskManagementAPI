using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Errors;

public class TaskErrors
{
    public static readonly Error EmptyTitle = Error.Validation(
        "Task.EmptyTitle",
        "Task title cannot be empty.");

    public static readonly Error InvalidPriority = Error.Validation(
        "Task.InvalidPriority",
        "Priority must be 1 (Low), 2 (Medium), or 3 (High).");

    public static readonly Error InvalidUserId = Error.Validation(
        "Task.InvalidUserId",
        "User ID must be positive.");

    public static Error NotFound(int taskId) => Error.NotFound(
        "Task.NotFound",
        $"Task with ID '{taskId}' was not found.");

    public static readonly Error AccessDenied = Error.Forbidden(
        "Task.AccessDenied",
        "You do not have permission to access this task.");

}
