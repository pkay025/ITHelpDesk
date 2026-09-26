using ITHelpDesk.Core.Contracts;
using ITHelpDesk.Core.Enums;

namespace ITHelpDesk.Web.Services;

public class AuthSession
{
    public string? Token { get; private set; }
    public string? Name { get; private set; }
    public string? Email { get; private set; }
    public IReadOnlyList<string> Roles { get; private set; } = [];
    public UserType UserType { get; private set; } = UserType.Student;
    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Token);
    public bool IsStaff => UserType == UserType.Staff || Roles.Contains(UserRole.SupportAgent) || Roles.Contains(UserRole.Administrator);
    public bool IsInitialized { get; set; }

    public event Action? OnChange;

    public void SignIn(AuthenticationResponse response)
    {
        Token = response.Token;
        Name = response.Name;
        Email = response.Email;
        Roles = response.Roles;
        UserType = response.UserType;
        IsInitialized = true;
        NotifyStateChanged();
    }

    public void SignOut()
    {
        Token = null;
        Name = null;
        Email = null;
        Roles = [];
        UserType = UserType.Student;
        IsInitialized = true;
        NotifyStateChanged();
    }

    public AuthenticationResponse? ToResponse() =>
        IsAuthenticated ? new(Token!, Name ?? string.Empty, Email ?? string.Empty, Roles, UserType) : null;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
