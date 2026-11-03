namespace TaskManagement.Application.DTOs;

public class LoginRequest
{
    public string email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}
