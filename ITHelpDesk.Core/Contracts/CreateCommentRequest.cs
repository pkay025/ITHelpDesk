using System.ComponentModel.DataAnnotations;

namespace ITHelpDesk.Core.Contracts;

public class CreateCommentRequest
{
    [Required, StringLength(120)]
    public string AuthorName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(320)]
    public string AuthorEmail { get; set; } = string.Empty;

    [Required, StringLength(4000)]
    public string Message { get; set; } = string.Empty;
}
