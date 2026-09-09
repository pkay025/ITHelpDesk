using ITHelpDesk.Core.Entities;

namespace ITHelpDesk.Tests;

public class TicketCommentTests
{
    [Fact]
    public void NewComment_InitializesCorrectly()
    {
        // Act
        var comment = new TicketComment
        {
            TicketId = 42,
            AuthorName = "Support Agent 1",
            AuthorEmail = "agent1@helpdesk.com",
            Message = "We are currently investigating the issue."
        };

        // Assert
        Assert.Equal(42, comment.TicketId);
        Assert.Equal("Support Agent 1", comment.AuthorName);
        Assert.Equal("agent1@helpdesk.com", comment.AuthorEmail);
        Assert.Equal("We are currently investigating the issue.", comment.Message);
        Assert.True(comment.CreatedAtUtc <= DateTime.UtcNow);
    }
}
