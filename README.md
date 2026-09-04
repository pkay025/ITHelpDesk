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

Authentication, role-based permissions, ticket comments, notifications, reporting, automated database migrations, and production deployment configuration are planned follow-up work.
