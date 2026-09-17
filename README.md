# IT Help Desk Management System

A web-based help desk system for submitting, tracking, assigning, and resolving technical support tickets.

## Technolog

- .NET 10
- ASP.NET Core Minimal Web API
- Blazor Web App with interactive server rendering
- Entity Framework Core
- xUnit test framework
- SQLite for local Development
- SQL Server for non-Development environments

## Solution Structure

```text
ITHelpDesk/
|-- ITHelpDesk.API/   Backend API and database access
|-- ITHelpDesk.Core/  Shared entities, enums, and request contracts
|-- ITHelpDesk.Web/   Blazor user interface
|-- ITHelpDesk.Tests/ Automated test suite (xUnit)
`-- ITHelpDesk.slnx   Solution file
```

## Features

- Help desk dashboard with ticket metrics and active counts
- Ticket submission form with automatic requester population for authenticated users
- Ticket detail view with full comment threads (create, list, delete comments)
- Staff queue for triaging, assigning ownership, and updating ticket status/priority
- User registration and login in the Blazor Web UI (`/register` and `/login`)
- Student and Staff user classification (`UserType`)
- ASP.NET Core Identity authentication and JWT token issuance
- Requester, support agent, and administrator roles
- Protected staff queue updates
- SQLite-backed local development database
- Comprehensive automated xUnit test suite covering lifecycle, contracts, and database persistence
- SQL Server configuration for deployment environments

## Requirements

- .NET SDK 10.0 or later
- SQL Server for non-Development environments

Check the installed SDK with:

```powershell
dotnet --version
```

## Run Locally

Start the API in one terminal:

```powershell
dotnet run --project .\ITHelpDesk.API --launch-profile http
```

The API runs at `http://localhost:5200`.

Start the Blazor Web app in a second terminal:

```powershell
dotnet run --project .\ITHelpDesk.Web --launch-profile http
```

Open the application at `http://localhost:5168`.

The API creates `ITHelpDesk.API/helpdesk.db` automatically in Development. This file is ignored by Git and persists tickets between API restarts.

Identity creates a separate `ITHelpDesk.API/auth.db` database for local users and roles. It is also ignored by Git.

## Authentication & Users

Users can register through the Web UI at `/register` or via `POST /api/auth/register`, selecting their account type (`Student` or `Staff`).

Sign in at `/login` or through `POST /api/auth/login` to receive a JWT.

The built-in roles are:

- `Requester`: can submit tickets and view their requests
- `SupportAgent`: can manage the staff queue and update tickets
- `Administrator`: can manage the staff queue and system configuration

Ticket updates require a JWT containing either the `SupportAgent` or `Administrator` role. The Web app attaches the bearer token to authorized API requests.

For local development, an administrator can be seeded through environment configuration without storing credentials in source control:

```powershell 
$env:Auth__AdminEmail = "admin@example.com"
$env:Auth__AdminPassword = "Use-a-local-password-123!"
$env:Auth__AdminName = "System Administrator"
dotnet run --project .\ITHelpDesk.API --launch-profile http
```

## API Endpoints

### Authentication

| Method | Endpoint             | Purpose                                           |
| ------ | -------------------- | ------------------------------------------------- |
| POST   | `/api/auth/register` | Register a new user (`Student` or `Staff`)        |
| POST   | `/api/auth/login`    | Sign in and obtain JWT token and role information |

### Tickets

| Method | Endpoint                         | Purpose                                |
| ------ | -------------------------------- | -------------------------------------- |
| GET    | `/api/tickets`                   | List tickets (supports `?status=` filter)|
| GET    | `/api/tickets/{id}`              | Get ticket details                     |
| POST   | `/api/tickets`                   | Create a new ticket                    |
| PATCH  | `/api/tickets/{id}`              | Update status, priority, or assignment |
| GET    | `/api/tickets/{id}/comments`     | List comments on a ticket              |
| POST   | `/api/tickets/{id}/comments`     | Add comment to a ticket                |
| DELETE | `/api/tickets/{id}/comments/{commentId}` | Delete a comment               |

## Build and Test

Build the complete solution:

```powershell
dotnet build .\ITHelpDesk.slnx
```

Run the automated test suite:

```powershell
dotnet test .\ITHelpDesk.slnx
```

## Database Configuration

Development uses SQLite through:

```text
Data Source=helpdesk.db
```

## Future Scope

Email notifications, advanced analytics/reporting export, automated migrations for production SQL Server, and container deployment configuration are planned follow-up enhancements.
