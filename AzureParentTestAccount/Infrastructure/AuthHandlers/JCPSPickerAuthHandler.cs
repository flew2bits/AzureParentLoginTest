using System.Text.Encodings.Web;
using AzureParentTestAccount.Infrastructure.AuthHandlers.Scheme;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace AzureParentTestAccount.Infrastructure.AuthHandlers;

public class JcpsPickerAuthHandler : AuthenticationHandler<JcpsPickerAuthSchemeOptions>
{
    public JcpsPickerAuthHandler(IOptionsMonitor<JcpsPickerAuthSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        var returnUrl = UrlEncoder.Encode($"{Request.PathBase}{Request.Path}{Request.QueryString}");
        
        Response.Redirect($"{Options.PickAuthenticationUrl}?returnUrl={returnUrl}" ?? throw new InvalidOperationException("authentication picker url can't be null"));
        return Task.CompletedTask;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync() =>
        Task.FromResult(AuthenticateResult.NoResult());
}
