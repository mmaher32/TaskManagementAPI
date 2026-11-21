using System.Globalization;
using System.Security.Claims;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Common;

namespace TaskManagement.API.EndPoints;

public static class TaskEndPoints
{
    public static void MapTaskEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").WithTags("Tasks").RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, TaskService service, CancellationToken ct) =>
            Results.Ok(await service.ListAsync(GetUserId(user), ct)));

        group.MapPost("/", async (CreateTaskRequest request, ClaimsPrincipal user, TaskService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(GetUserId(user), request, ct);
            return result.IsSuccess ? Results.Json(result.Value, statusCode: 201) : Problem(result.Error!);
        });

        group.MapPut("/{id:int}", async (int id, UpdateTaskRequest request, ClaimsPrincipal user, TaskService service, CancellationToken ct) =>
        {
            var result = await service.UpdateAsync(GetUserId(user), id, request, ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Problem(result.Error!);
        });

        group.MapDelete("/{id:int}", async (int id, ClaimsPrincipal user, TaskService service, CancellationToken ct) =>
        {
            var result = await service.DeleteAsync(GetUserId(user), id, ct);
            return result.IsSuccess ? Results.NoContent() : Problem(result.Error!);
        });
    }

    // The bearer handler validates the subject before an endpoint runs.
    private static int GetUserId(ClaimsPrincipal user) =>
        int.Parse(user.FindFirstValue("sub")!, CultureInfo.InvariantCulture);

    private static IResult Problem(Error error) => Results.Problem(
        statusCode: error.Type == ErrorType.NotFound ? 404 : 400,
        title: error.Code, detail: error.Message);
}
