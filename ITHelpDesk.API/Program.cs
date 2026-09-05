using ITHelpDesk.API.Data;
using ITHelpDesk.Core.Contracts;
using ITHelpDesk.Core.Entities;
using ITHelpDesk.Core.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<HelpDeskDbContext>(options => options.UseSqlite("Data Source=helpdesk.db"));
}
else
{
    builder.Services.AddDbContext<HelpDeskDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("HelpDesk")));
}

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<HelpDeskDbContext>()
.AddSignInManager();

var jwtKey = builder.Configuration["Jwt:Key"] ??
    Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey
        };
    });
builder.Services.AddAuthorization();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/auth/register", async (RegisterRequest request, UserManager<ApplicationUser> users, RoleManager<IdentityRole> roles) =>
{
    var user = new ApplicationUser
    {
        UserName = request.Email.Trim(),
        Email = request.Email.Trim(),
        DisplayName = request.Name.Trim()
    };

    var result = await users.CreateAsync(user, request.Password);
    if (!result.Succeeded)
    {
        return Results.ValidationProblem(result.Errors.ToDictionary(error => error.Code, error => new[] { error.Description }));
    }

    await EnsureRoleAsync(roles, UserRole.Requester);
    await users.AddToRoleAsync(user, UserRole.Requester);
    return Results.Ok(new { message = "Account created." });
});

app.MapPost("/api/auth/login", async (LoginRequest request, UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signInManager) =>
{
    var user = await users.FindByEmailAsync(request.Email.Trim());
    if (user is null || !(await signInManager.CheckPasswordSignInAsync(user, request.Password, false)).Succeeded)
    {
        return Results.Unauthorized();
    }

    var roles = await users.GetRolesAsync(user);
    var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id),
        new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        new(ClaimTypes.Name, user.DisplayName)
    };
    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
    var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
    return Results.Ok(new AuthenticationResponse(new JwtSecurityTokenHandler().WriteToken(token), user.DisplayName, user.Email ?? string.Empty, roles.ToArray()));
});

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
}).RequireAuthorization(policy => policy.RequireRole(UserRole.SupportAgent, UserRole.Administrator));

app.Run();

static async Task EnsureRoleAsync(RoleManager<IdentityRole> roles, string role)
{
    if (!await roles.RoleExistsAsync(role))
    {
        await roles.CreateAsync(new IdentityRole(role));
    }
}
