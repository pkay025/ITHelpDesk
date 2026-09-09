using ITHelpDesk.Core.Entities;
using ITHelpDesk.Core.Enums;

namespace ITHelpDesk.Tests;

public class TicketLifecycleTests
{
    [Fact]
    public void NewTicket_HasDefaultValues()
    {
        // Act
        var ticket = new Ticket
        {
            Title = "Network connection issue",
            Description = "Cannot connect to campus Wi-Fi",
            RequesterName = "Alice Smith",
            RequesterEmail = "alice@example.com"
        };

        // Assert
        Assert.Equal(TicketStatus.Open, ticket.Status);
        Assert.Equal(TicketPriority.Medium, ticket.Priority);
        Assert.Null(ticket.AssignedTo);
        Assert.Null(ticket.ResolvedAtUtc);
        Assert.Null(ticket.UpdatedAtUtc);
        Assert.True(ticket.CreatedAtUtc <= DateTime.UtcNow);
    }

    [Fact]
    public void Ticket_CanBeAssignedAndProgressed()
    {
        // Arrange
        var ticket = new Ticket
        {
            Title = "Printer failure",
            Description = "Paper jam in room 302",
            RequesterName = "Bob Jones",
            RequesterEmail = "bob@example.com"
        };

        // Act
        ticket.AssignedTo = "Agent John";
        ticket.Status = TicketStatus.InProgress;
        ticket.UpdatedAtUtc = DateTime.UtcNow;

        // Assert
        Assert.Equal("Agent John", ticket.AssignedTo);
        Assert.Equal(TicketStatus.InProgress, ticket.Status);
        Assert.NotNull(ticket.UpdatedAtUtc);
    }

    [Fact]
    public void Ticket_WhenResolved_RecordsResolutionTimestamp()
    {
        // Arrange
        var ticket = new Ticket
        {
            Title = "Software installation",
            Description = "Requesting VS Code installation",
            RequesterName = "Charlie Brown",
            RequesterEmail = "charlie@example.com"
        };

        // Act
        var resolutionTime = DateTime.UtcNow;
        ticket.Status = TicketStatus.Resolved;
        ticket.ResolvedAtUtc = resolutionTime;

        // Assert
        Assert.Equal(TicketStatus.Resolved, ticket.Status);
        Assert.Equal(resolutionTime, ticket.ResolvedAtUtc);
    }
}
