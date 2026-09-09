using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ITHelpDesk.API.Data;
using ITHelpDesk.Core.Entities;
using ITHelpDesk.Core.Enums;

namespace ITHelpDesk.Tests;

public class DbContextTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly HelpDeskDbContext _context;

    public DbContextTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<HelpDeskDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new HelpDeskDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task CanAddAndRetrieveTicket()
    {
        var ticket = new Ticket
        {
            Title = "Cannot access email",
            Description = "Getting 403 error",
            RequesterName = "David Lee",
            RequesterEmail = "david@example.com",
            Priority = TicketPriority.High
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        var retrieved = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticket.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Cannot access email", retrieved.Title);
        Assert.Equal(TicketPriority.High, retrieved.Priority);
    }

    [Fact]
    public async Task CanAddAndRetrieveCommentsForTicket()
    {
        var ticket = new Ticket
        {
            Title = "Wi-Fi down",
            Description = "Library Wi-Fi not working",
            RequesterName = "Emma Watson",
            RequesterEmail = "emma@example.com"
        };
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        var comment = new TicketComment
        {
            TicketId = ticket.Id,
            AuthorName = "Support Agent",
            AuthorEmail = "agent@example.com",
            Message = "Restarting access point now."
        };
        _context.TicketComments.Add(comment);
        await _context.SaveChangesAsync();

        var comments = await _context.TicketComments
            .Where(c => c.TicketId == ticket.Id)
            .ToListAsync();

        Assert.Single(comments);
        Assert.Equal("Restarting access point now.", comments[0].Message);
    }

    [Fact]
    public async Task CanFilterTicketsByRequesterEmail()
    {
        _context.Tickets.AddRange(
            new Ticket { Title = "T1", Description = "D1", RequesterName = "User A", RequesterEmail = "usera@example.com" },
            new Ticket { Title = "T2", Description = "D2", RequesterName = "User B", RequesterEmail = "userb@example.com" }
        );
        await _context.SaveChangesAsync();

        var userATickets = await _context.Tickets
            .Where(t => t.RequesterEmail == "usera@example.com")
            .ToListAsync();

        Assert.Single(userATickets);
        Assert.Equal("T1", userATickets[0].Title);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
