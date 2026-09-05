using System.ComponentModel.DataAnnotations;

namespace ITHelpDesk.Core.Contracts;

public class RegisterRequest
{
    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(320)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public record AuthenticationResponse(string Token, string Name, string Email, IReadOnlyList<string> Roles);
