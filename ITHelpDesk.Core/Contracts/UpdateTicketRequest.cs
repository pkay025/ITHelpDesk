using System.ComponentModel.DataAnnotations;
using ITHelpDesk.Core.Enums;

namespace ITHelpDesk.Core.Contracts;

public class UpdateTicketRequest
{
    public TicketStatus Status { get; set; }

    public TicketPriority Priority { get; set; }

    [StringLength(120)]
    public string? AssignedTo { get; set; }
}
