using System.ComponentModel.DataAnnotations;
using ITHelpDesk.Core.Contracts;
using ITHelpDesk.Core.Enums;

namespace ITHelpDesk.Tests;

public class AuthenticationContractTests
{
    [Fact]
    public void RegisterRequest_DefaultUserType_IsStudent()
    {
        var request = new RegisterRequest();
        Assert.Equal(UserType.Student, request.UserType);
    }

    [Fact]
    public void RegisterRequest_ValidationFails_WhenFieldsAreMissing()
    {
        var request = new RegisterRequest();
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(request, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequest.Name)));
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequest.Email)));
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequest.Password)));
    }

    [Fact]
    public void RegisterRequest_ValidationFails_WhenPasswordIsTooShort()
    {
        var request = new RegisterRequest
        {
            Name = "Alice",
            Email = "alice@example.com",
            Password = "short" // < 8 characters
        };
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(request, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterRequest.Password)));
    }

    [Fact]
    public void RegisterRequest_ValidationPasses_WhenAllFieldsAreValid()
    {
        var request = new RegisterRequest
        {
            Name = "Alice Smith",
            Email = "alice@example.com",
            Password = "ValidPassword123!",
            UserType = UserType.Staff
        };
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(request, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void AuthenticationResponse_StoresAllPropertiesCorrectly()
    {
        var response = new AuthenticationResponse(
            Token: "jwt-token-xyz",
            Name: "Test User",
            Email: "test@example.com",
            Roles: [UserRole.SupportAgent],
            UserType: UserType.Staff
        );

        Assert.Equal("jwt-token-xyz", response.Token);
        Assert.Equal("Test User", response.Name);
        Assert.Equal("test@example.com", response.Email);
        Assert.Single(response.Roles);
        Assert.Equal(UserRole.SupportAgent, response.Roles[0]);
        Assert.Equal(UserType.Staff, response.UserType);
    }
}
