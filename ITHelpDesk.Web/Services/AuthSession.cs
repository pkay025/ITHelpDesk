using ITHelpDesk.Core.Contracts;

namespace ITHelpDesk.Web.Services;

public class AuthSession
{
    public string? Token { get; private set; }
    public string? Name { get; private set; }
    public string? Email { get; private set; }
    public IReadOnlyList<string> Roles { get; private set; } = [];
    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Token);
    public bool IsStaff => Roles.Contains(UserRole.SupportAgent) || Roles.Contains(UserRole.Administrator);

    public void SignIn(AuthenticationResponse response)
    {
        Token = response.Token;
        Name = response.Name;
        Email = response.Email;
        Roles = response.Roles;
    }

    public void SignOut()
    {
        Token = null;
        Name = null;
        Email = null;
        Roles = [];
    }
}
