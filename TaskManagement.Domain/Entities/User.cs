using TaskManagement.Domain.Common;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Domain.Entities;

public class User
{
    public const int MaxUserNameLength = 100;
    public const int MaxEmailLength = 254;
    public int Id { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    private readonly List<TaskItem> _tasks = new();
    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();
    public int RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
    private User() { }
    public static Result<User> Create(
        string userName,
        string email,
        string passwordHash,
        Role role)
    {
        var nameResult = ValidateUserName(userName);

        if (nameResult.IsFailure)
            return Result<User>.Failure(nameResult.Error!);

        var emailResult = ValidateEmail(email);

        if (emailResult.IsFailure)
            return Result<User>.Failure(emailResult.Error!);

        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result<User>.Failure(UserErrors.EmptyPasswordHash);

        var user = new User
        {
            UserName = userName.Trim(),
            Email = email.Trim(),
            PasswordHash = passwordHash,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        return Result<User>.Success(user);
    }
    public Result ChangeUserName(string userName)
    {
        var result = ValidateUserName(userName);

        if (result.IsFailure)
            return result;

        UserName = userName.Trim();

        return Result.Success();
    }

    public Result ChangeEmail(string email)
    {
        var result = ValidateEmail(email);

        if (result.IsFailure)
            return result;

        Email = email.Trim();

        return Result.Success();
    }

    public Result ChangePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result.Failure(UserErrors.EmptyPasswordHash);

        PasswordHash = passwordHash;

        return Result.Success();
    }

    private static Result ValidateUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return Result.Failure(UserErrors.EmptyUserName);

        if (userName.Trim().Length > MaxUserNameLength)
            return Result.Failure(UserErrors.UserNameTooLong);

        return Result.Success();
    }

    private static Result ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure(UserErrors.EmptyEmail);

        var trimmedEmail = email.Trim();

        if (trimmedEmail.Length > MaxEmailLength)
            return Result.Failure(UserErrors.EmailTooLong);

        var atIndex = trimmedEmail.IndexOf('@');

        if (trimmedEmail.Any(char.IsWhiteSpace) ||
            atIndex <= 0 ||
            atIndex != trimmedEmail.LastIndexOf('@') ||
            atIndex == trimmedEmail.Length - 1)
        {
            return Result.Failure(UserErrors.InvalidEmail);
        }

        return Result.Success();
    }
}
