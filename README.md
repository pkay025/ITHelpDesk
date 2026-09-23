# IT Help Desk — Backend API Service

This branch (`backend`) contains the standalone REST API and domain logic for the IT Help Desk Management System.

## Architecture

- **`ITHelpDesk.API/`**: ASP.NET Core Minimal Web API (.NET 10), EF Core, ASP.NET Core Identity & JWT Bearer authentication.
- **`ITHelpDesk.Core/`**: Shared entities, enums, and request contracts.
- **`ITHelpDesk.Tests/`**: Automated xUnit test suite for business logic and data persistence.

## Run Locally

```powershell
dotnet run --project .\ITHelpDesk.API --launch-profile http
```
The API listens at `http://localhost:5200`.

To seed an initial admin account locally:
```powershell
$env:Auth__AdminEmail = "admin@example.com"
$env:Auth__AdminPassword = "AdminPassword123!"
$env:Auth__AdminName = "Lead Administrator"
dotnet run --project .\ITHelpDesk.API --launch-profile http
```

## Run Automated Tests

```powershell
dotnet test .\ITHelpDesk.Tests\ITHelpDesk.Tests.csproj
```

## Container & Cloud Deployment (Docker / Render / Railway / Fly.io)

### Build and Run with Docker

```bash
docker build -t ithelpdesk-api .
docker run -d -p 5200:8080 -e ASPNETCORE_ENVIRONMENT=Production -e Jwt__Key="YOUR_SUPER_SECRET_KEY_AT_LEAST_32_CHARS" ithelpdesk-api
```

### Environment Variables for Production

| Variable | Description | Example |
| :--- | :--- | :--- |
| `ASPNETCORE_ENVIRONMENT` | Hosting environment | `Production` |
| `ASPNETCORE_HTTP_PORTS` | Port inside container | `8080` |
| `Jwt__Key` | Secret key for JWT signing (>= 32 chars) | `SuperSecretProductionKey1234567890!` |
| `Cors__AllowedOrigins__0` | Deployed Frontend URL | `https://your-frontend.onrender.com` |
| `ConnectionStrings__HelpDesk` | Production database connection string | `Server=...;Database=...` |
| `Auth__AdminEmail` | Auto-seed admin email on initial startup | `admin@example.com` |
| `Auth__AdminPassword` | Auto-seed admin password | `SecureAdmin123!` |
| `Auth__AdminName` | Auto-seed admin display name | `System Admin` |

### Deploying to Render / Railway
1. Create a new **Web Service**.
2. Connect your GitHub repository and select the **`backend`** branch.
3. Select **Docker** as the environment (Render/Railway will automatically detect the root `Dockerfile`).
4. Add the required Environment Variables in the platform dashboard (`Jwt__Key`, `Cors__AllowedOrigins__0`, etc.).
5. Deploy!
