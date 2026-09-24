# IT Help Desk — Frontend Web Application

This branch (`frontend`) contains the Blazor Web application for the IT Help Desk Management System.

## Architecture

- **`ITHelpDesk.Web/`**: Interactive Blazor Web App (.NET 10 Server-side rendering), responsive Bootstrap 5 UI, client auth session, and API integration.
- **`ITHelpDesk.Core/`**: Shared entities, enums, and request contracts.

## Connecting to the Backend API

The web application communicates with the backend REST API via an `HttpClient`.

The backend address is configured through the **`ApiBaseUrl`** configuration key:

- **Local Development**: Default is `http://localhost:5200` in `ITHelpDesk.Web/appsettings.json`.
- **Production / Deployed**: Override with the environment variable `ApiBaseUrl` pointing to your deployed backend API URL (e.g., `https://ithelpdesk-api.onrender.com`).

## Run Locally

```powershell
dotnet run --project .\ITHelpDesk.Web --launch-profile http
```
The application opens at `http://localhost:5168`.

*Make sure your backend API is running (either locally or on your deployed cloud host) so data loads properly.*

## Container & Cloud Deployment (Docker / Render / Railway / Fly.io)

### Build and Run with Docker

```bash
docker build -t ithelpdesk-web .
docker run -d -p 5168:8080 -e ASPNETCORE_ENVIRONMENT=Production -e ApiBaseUrl="https://YOUR_DEPLOYED_BACKEND_URL" ithelpdesk-web
```

### Environment Variables for Production

| Variable | Description | Example |
| :--- | :--- | :--- |
| `ASPNETCORE_ENVIRONMENT` | Hosting environment | `Production` |
| `ASPNETCORE_HTTP_PORTS` | Container listening port | `8080` |
| `ApiBaseUrl` | URL of the deployed backend API | `https://ithelpdesk-api.onrender.com`