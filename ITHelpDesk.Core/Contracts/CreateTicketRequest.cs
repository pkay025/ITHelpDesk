using System.ComponentModel.DataAnnotations;
using ITHelpDesk.Core.Enums;

namespace ITHelpDesk.Core.Contracts;

public class CreateTicketRequest
{
    [Required, StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(4000)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string RequesterName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(320)]
    public string RequesterEmail { get; set; } = string.Empty;

    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
}
