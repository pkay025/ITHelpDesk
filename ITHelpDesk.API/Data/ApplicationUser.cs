using Microsoft.AspNetCore.Identity;
using ITHelpDesk.Core.Enums;

namespace ITHelpDesk.API.Data;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public UserType UserType { get; set; } = UserType.Student;
}
