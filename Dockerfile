# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore
COPY ["ITHelpDesk.Core/ITHelpDesk.Core.csproj", "ITHelpDesk.Core/"]
COPY ["ITHelpDesk.Web/ITHelpDesk.Web.csproj", "ITHelpDesk.Web/"]
RUN dotnet restore "ITHelpDesk.Web/ITHelpDesk.Web.csproj"

# Copy source code and build Release
COPY ITHelpDesk.Core/ ITHelpDesk.Core/
COPY ITHelpDesk.Web/ ITHelpDesk.Web/
WORKDIR "/src/ITHelpDesk.Web"
RUN dotnet publish "ITHelpDesk.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ITHelpDesk.Web.dll"]
