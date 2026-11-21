# Task Management API

A small ASP.NET Core Minimal API for managing personal tasks. Users register, sign in, and use a 15-minute JWT access token to create, list, update, and delete their own tasks. Data is stored in PostgreSQL through Entity Framework Core.

## Run locally

Install the .NET 10 SDK and start PostgreSQL. Use a PostgreSQL account that can create a database, or create an empty database it owns. Run the following commands in PowerShell from the solution directory.

```powershell
dotnet restore
dotnet tool install --global dotnet-ef --version 10.0.14
```

If a 10.x version of `dotnet-ef` is already installed, skip the installation command. Set the connection string for your local database and generate a signing key. Replace the database credentials below before running the commands.

```powershell
$api = ".\TaskManagementAPI\TaskManagement.API.csproj"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=task_management;Username=YOUR_DB_USER;Password=YOUR_DB_PASSWORD" --project $api

$keyBytes = New-Object byte[] 32
$rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
$rng.GetBytes($keyBytes)
$rng.Dispose()
dotnet user-secrets set "Jwt:SigningKey" ([Convert]::ToBase64String($keyBytes)) --project $api

$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet ef database update --project .\TaskManagement.Infrastructure --startup-project $api
dotnet run --project $api --launch-profile http
```

Open [Swagger UI](http://localhost:5069/swagger) to inspect the routes. The local HTTP profile listens on `http://localhost:5069`.

## Try it in Postman

1. Send `POST /api/auth/register` with a JSON body containing `username`, `email`, and `password`.
2. Send `POST /api/auth/login` with the same `email` and `password`. Copy the returned `accessToken`.
3. For the requests below, select **Authorization → Bearer Token** and paste the token. Send JSON bodies with `Content-Type: application/json`. Sign in again when the token expires.

| Method | Path | Successful response |
| --- | --- | --- |
| GET | `/api/auth/me` | 200 with the authenticated user ID |
| POST | `/api/tasks` | 201 with the new task |
| GET | `/api/tasks` | 200 with your tasks, newest ID first |
| PUT | `/api/tasks/{id}` | 200 with the updated task |
| DELETE | `/api/tasks/{id}` | 204 with an empty body |

Create a task with `POST /api/tasks`. Only the title is required. Priority defaults to 1 and accepts values from 1 to 3. New tasks are incomplete. The API takes the owner ID from the token.

```json
{
  "title": "Read the EF Core chapter",
  "description": "Take notes on change tracking",
  "priority": 2,
  "dueDate": "2027-01-10T12:00:00Z"
}
```

Use the returned task ID in `PUT /api/tasks/{id}`. Send the complete editable state. Omitting the optional description, priority, or due date resets it to its default.

```json
{
  "title": "Read the EF Core chapter",
  "description": "Notes saved",
  "priority": 2,
  "dueDate": null,
  "isCompleted": true
}
```

Protected routes return 401 for a missing or invalid token. Updating or deleting a missing task or another user's task returns 404. A blank title or a priority outside 1–3 returns 400.

## Project structure

- **TaskManagement.Domain** contains entities, task rules, and result and error types.
- **TaskManagement.Application** contains use cases, DTOs, and repository contracts.
- **TaskManagement.Infrastructure** contains PostgreSQL mappings, migrations, repositories, password hashing, and token generation.
- **TaskManagementAPI** hosts the HTTP endpoints and configures authentication and dependency injection.
