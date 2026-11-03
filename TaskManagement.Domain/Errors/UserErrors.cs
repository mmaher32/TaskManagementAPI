using TaskManagement.Domain.Common;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Errors;

public class UserErrors
{
    public static readonly Error EmptyUserName = Error.Validation(
        "User.EmptyUserName",
        "Username cannot be empty.");

    public static readonly Error UserNameTooLong = Error.Validation(
        "User.UserNameTooLong",
        $"Username cannot exceed {User.MaxUserNameLength} characters.");

    public static readonly Error EmptyEmail = Error.Validation(
        "User.EmptyEmail",
        "Email cannot be empty.");

    public static readonly Error EmailTooLong = Error.Validation(
        "User.EmailTooLong",
        $"Email cannot exceed {User.MaxEmailLength} characters.");

    public static readonly Error InvalidEmail = Error.Validation(
        "User.InvalidEmail",
        "Email format is invalid.");

    public static readonly Error EmptyPasswordHash = Error.Validation(
        "User.EmptyPasswordHash",
        "Password hash cannot be empty.");

    public static readonly Error EmailAlreadyExists = Error.Conflict(
        "User.EmailAlreadyExists",
        "A user with this email already exists.");

    public static readonly Error UserNameAlreadyExists = Error.Conflict(
        "User.UserNameAlreadyExists",
        "A user with this username already exists.");

    public static Error NotFound(int userId) => Error.NotFound(
        "User.NotFound",
        $"User with ID '{userId}' was not found.");

    public static Error MissingCredentials = Error.Validation(
    "User.MissingCredentials",
    "Email and password are required.");

    public static Error InvalidCredentials = Error.Unauthorized(
        "User.InvalidCredentials",
        "Invalid email or password.");
}
