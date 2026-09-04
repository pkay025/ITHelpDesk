using ITHelpDesk.API.Data;
using ITHelpDesk.Core.Contracts;
using ITHelpDesk.Core.Entities;
using ITHelpDesk.Core.Enums;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<HelpDeskDbContext>(options => options.UseInMemoryDatabase("ITHelpDesk"));
}
else
{
    builder.Services.AddDbContext<HelpDeskDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("HelpDesk")));
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<HelpDeskDbContext>();
    database.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/api/tickets", async (HelpDeskDbContext database, TicketStatus? status) =>
{
    var query = database.Tickets.AsNoTracking().AsQueryable();
    if (status is not null)
    {
        query = query.Where(ticket => ticket.Status == status);
    }

    return Results.Ok(await query.OrderByDescending(ticket => ticket.CreatedAtUtc).ToListAsync());
});

app.MapGet("/api/tickets/{id:int}", async (int id, HelpDeskDbContext database) =>
{
    var ticket = await database.Tickets.AsNoTracking().FirstOrDefaultAsync(ticket => ticket.Id == id);
    return ticket is null ? Results.NotFound() : Results.Ok(ticket);
});

app.MapPost("/api/tickets", async (CreateTicketRequest request, HelpDeskDbContext database) =>
{
    var ticket = new Ticket
    {
        Title = request.Title.Trim(),
        Description = request.Description.Trim(),
        RequesterName = request.RequesterName.Trim(),
        RequesterEmail = request.RequesterEmail.Trim(),
        Priority = request.Priority,
        Status = TicketStatus.Open,
        CreatedAtUtc = DateTime.UtcNow
    };

    database.Tickets.Add(ticket);
    await database.SaveChangesAsync();
    return Results.Created($"/api/tickets/{ticket.Id}", ticket);
});

app.MapPatch("/api/tickets/{id:int}", async (int id, UpdateTicketRequest request, HelpDeskDbContext database) =>
{
    var ticket = await database.Tickets.FirstOrDefaultAsync(ticket => ticket.Id == id);
    if (ticket is null)
    {
        return Results.NotFound();
    }

    ticket.Status = request.Status;
    ticket.Priority = request.Priority;
    ticket.AssignedTo = string.IsNullOrWhiteSpace(request.AssignedTo) ? null : request.AssignedTo.Trim();
    ticket.UpdatedAtUtc = DateTime.UtcNow;
    ticket.ResolvedAtUtc = request.Status is TicketStatus.Resolved or TicketStatus.Closed
        ? ticket.ResolvedAtUtc ?? DateTime.UtcNow
        : null;

    await database.SaveChangesAsync();
    return Results.Ok(ticket);
});

app.Run();
