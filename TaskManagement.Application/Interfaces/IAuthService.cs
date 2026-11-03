using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Common;

namespace TaskManagement.Application.Interfaces;

public interface IAuthService
{
    Task<Result<UserResponse>> RegisterAsync(RegisterResponse request, CancellationToken ct);

    Task<Result<UserResponse>> LoginAsync(LoginRequest request, CancellationToken ct);
}
