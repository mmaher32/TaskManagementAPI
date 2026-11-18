using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces;

public interface IAccessTokenGenerator
{
    (string AccessToken, DateTimeOffset ExpiresAtUtc) Generate(User user);
}
