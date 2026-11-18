using System.ComponentModel.DataAnnotations;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Application.Services;

public class AuthService(
    IUserRepository _userRepo,
    IRoleRepository _roleRepo,
    IPasswordHasher _passHasher,
    IUnitOfWork _uow) : IAuthService
{
    public async Task<Result<UserResponse>> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var email = request.email?.Trim() ?? string.Empty;
        var password = request.Password;

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            return Result<UserResponse>.Failure(
                UserErrors.MissingCredentials);
        }

        var user = await _userRepo.GetByEmailAsync(email, ct);

        if (user is null ||
            !_passHasher.verify(password, user.PasswordHash))
        {
            return Result<UserResponse>.Failure(
                UserErrors.InvalidCredentials);
        }

        return Result<UserResponse>.Success(new UserResponse
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            Role = user.Role.Name
        });
    }

    public async Task<Result<UserResponse>> RegisterAsync(RegisterResponse request, CancellationToken ct)
    {
        var username = request.Username?.Trim() ?? string.Empty;
        var email = request.Email?.Trim() ?? string.Empty;
        var password = request.Password;

        var emailExists = await _userRepo.EmailExistsAsync(email, ct);

        if (emailExists)
        {
            return Result<UserResponse>.Failure(
                UserErrors.EmailAlreadyExists);
        }

        if (username.Length < 3 || username.Length > 50)
        {
            return Result<UserResponse>.Failure(
                Error.Validation("Registration.InvalidUsername",
                "Username must be between 3 and 50 characters"));
        }
        if (String.IsNullOrWhiteSpace(email) || email.Length > 254 ||
            !new EmailAddressAttribute().IsValid(email))
        {
            return Result<UserResponse>.Failure(
                Error.Validation(
                    "Registration.InvalidEmail",
                    "A valid email of at most 254 characters is required."));
        }
        if (string.IsNullOrWhiteSpace(password) ||
            password.Length < 6 ||
            password.Length > 100)
        {
            return Result<UserResponse>.Failure(
                Error.Validation(
                    "Registration.InvalidPassword",
                    "Password must be between 6 and 100 characters."));
        }

        var usernameExists = await _userRepo.UsernameExistsAsync(username, ct);
        if (usernameExists)
        {
            return Result<UserResponse>.Failure(UserErrors.UserNameAlreadyExists);
        }
        var userDefaultRole = await _roleRepo.GetByNameAsync(RoleConstants.User, ct);
        if (userDefaultRole is null)
        {
            return Result<UserResponse>.Failure(
                Error.Failure("Registration.DefaultRoleMissing",
                "The default user role is not configured"));
        }

        var hashedPassword = _passHasher.Hash(password);

        var userResult = User.Create(
            username,
            email,
            hashedPassword,
            userDefaultRole);

        if (userResult.IsFailure)
        {
            return Result<UserResponse>.Failure(userResult.Error!);
        }

        var newUser = userResult.Value;

        _userRepo.Add(newUser);

        await _uow.SaveChangesAsync(ct);

        var response = new UserResponse
        {
            Id = newUser.Id,
            Username = newUser.UserName,
            Email = newUser.Email,
            CreatedAt = newUser.CreatedAt,
            Role = newUser.Role.Name,
        };
        return Result<UserResponse>.Success(response);
    }
}
