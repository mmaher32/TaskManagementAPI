using TaskManagement.Application.Interfaces;

namespace TaskManagement.Infrastructure.Security;

public sealed class PasswordHash : IPasswordHasher
{
    public string Hash(string password)
    => CryptoHelper.PasswordHasher.HashPassword(password);
}
