using Microsoft.AspNetCore.Authentication;

namespace AzureParentTestAccount.Infrastructure.AuthHandlers.Scheme;

public class JcpsPickerAuthSchemeOptions : AuthenticationSchemeOptions
{
    public string? PickAuthenticationUrl { get; set; }
}