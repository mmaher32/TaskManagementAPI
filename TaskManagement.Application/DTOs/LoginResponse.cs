namespace TaskManagement.Application.DTOs;

public class LoginResponse
{
    public required UserResponse User { get; init; }
    public required string AccessToken { get; init; }
    public required DateTimeOffset ExpiresAtUtc { get; init; }
}