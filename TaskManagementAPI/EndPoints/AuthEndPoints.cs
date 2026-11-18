using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Common;

namespace TaskManagement.API.EndPoints;

public static class AuthEndPoints
{
    public static IEndpointRouteBuilder MapAuthEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth")
            .WithTags("Auth");

        group.MapPost("/register", async (
            RegisterResponse request,
            IAuthService authService,
            CancellationToken ct
            ) =>
        {
            var result = await authService.RegisterAsync(request, ct);
            if (result.IsFailure)
            {
                var statusCode = result.Error!.Type switch
                {
                    ErrorType.Validation => StatusCodes.Status400BadRequest,
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                };

                return Results.Problem(
                    statusCode: statusCode,
                    title: result.Error.Code,
                     detail: statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected server error occurred."
                : result.Error.Message);
            }
            return Results.Json(
                result.Value,
                statusCode: StatusCodes.Status201Created
                );
        });

        group.MapPost("/login", async (
            LoginRequest request,
            IAuthService authService,
            CancellationToken ct) =>
        {
            var result = await authService.LoginAsync(request, ct);
            if (result.IsFailure)
            {
                var statusCode = result.Error!.Type switch
                {
                    ErrorType.Validation => StatusCodes.Status400BadRequest,
                    ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };

                return Results.Problem(
                    statusCode: statusCode,
                    title: result.Error.Code,
                    detail: statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected server error occurred."
                : result.Error.Message
                        );
            }
            return Results.Ok(result.Value);
        });
        return app;
    }
}
