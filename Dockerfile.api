# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore
COPY ["ITHelpDesk.Core/ITHelpDesk.Core.csproj", "ITHelpDesk.Core/"]
COPY ["ITHelpDesk.API/ITHelpDesk.API.csproj", "ITHelpDesk.API/"]
RUN dotnet restore "ITHelpDesk.API/ITHelpDesk.API.csproj"

# Copy source code and build Release
COPY ITHelpDesk.Core/ ITHelpDesk.Core/
COPY ITHelpDesk.API/ ITHelpDesk.API/
WORKDIR "/src/ITHelpDesk.API"
RUN dotnet publish "ITHelpDesk.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ITHelpDesk.API.dll"]
