using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Security;

public sealed class JwtAccessTokenGenerator(
    string issuer,
    string audience,
    SigningCredentials signingCredentials) : IAccessTokenGenerator
{
    public (string AccessToken, DateTimeOffset ExpiresAtUtc) Generate(User user)
    {
        var now = DateTimeOffset.UtcNow;
        var expiresAtUtc = now.AddMinutes(15);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            IssuedAt = now.UtcDateTime,
            NotBefore = now.UtcDateTime,
            Expires = expiresAtUtc.UtcDateTime,
            SigningCredentials = signingCredentials,
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] =
                    user.Id.ToString(CultureInfo.InvariantCulture)
            }
        };

        var accessToken = new JsonWebTokenHandler().CreateToken(descriptor);
        return (accessToken, expiresAtUtc);
    }
}
