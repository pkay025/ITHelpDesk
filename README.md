# IT Help Desk Management System

A web-based help desk system for submitting, tracking, assigning, and resolving technical support tickets.

## Technology

- .NET 10
- ASP.NET Core Minimal Web API
- Blazor Web App with interactive server rendering
- Entity Framework Core
- SQLite for local Development
- SQL Server for non-Development environments

## Solution Structure

```text
ITHelpDesk/
|-- ITHelpDesk.API/   Backend API and database access
|-- ITHelpDesk.Core/  Shared entities, enums, and request contracts
|-- ITHelpDesk.Web/   Blazor user interface
`-- ITHelpDesk.slnx   Solution file
```

## Features

- Help desk dashboard with ticket metrics
- Ticket submission form
- Staff queue for reviewing requests
- Ticket status, priority, and assignment updates
- ASP.NET Core Identity registration and JWT login
- Requester, support agent, and administrator roles
- Protected staff queue updates
- SQLite-backed local development database
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

## Authentication

Create a requester account with `POST /api/auth/register`, then sign in through `POST /api/auth/login` to receive a JWT.

The built-in roles are:

- `Requester`: can submit tickets
- `SupportAgent`: can manage the staff queue
- `Administrator`: can manage the staff queue and future administrative features

Ticket updates require a JWT containing either the `SupportAgent` or `Administrator` role. The Web app provides the `/login` page and attaches the token to API requests.

For local development, an administrator can be seeded through environment configuration without storing credentials in source control:

```powershell
$env:Auth__AdminEmail = "admin@example.com"
$env:Auth__AdminPassword = "Use-a-local-password-123!"
$env:Auth__AdminName = "System Administrator"
dotnet run --project .\ITHelpDesk.API --launch-profile http
```

## Ticket API

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/tickets` | List tickets |
| GET | `/api/tickets/{id}` | Get one ticket |
| POST | `/api/tickets` | Create a ticket |
| PATCH | `/api/tickets/{id}` | Update status, priority, or assignment |

The list endpoint also accepts an optional status filter, for example:

```text
http://localhost:5200/api/tickets?status=Open
```

## Build and Validate

Build the complete solution:

```powershell
dotnet build .\ITHelpDesk.slnx
```

## Database Configuration

Development uses SQLite through:

```text
Data Source=helpdesk.db
```

## Current Scope

Ticket comments, notifications, reporting, automated database migrations, and production deployment configuration are planned follow-up work.
