namespace ITHelpDesk.Core.Entities;

public class TicketComment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorEmail { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
