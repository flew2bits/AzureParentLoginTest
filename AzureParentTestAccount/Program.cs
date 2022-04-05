using AzureParentTestAccount.Infrastructure.AuthHandlers;
using AzureParentTestAccount.Infrastructure.AuthHandlers.Scheme;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
    {
        options.Conventions.AuthorizePage("/Secret");
        options.Conventions.AuthorizePage("/LoginPa");
        options.Conventions.AuthorizePage("/LoginAz");
    })
    .Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "AzureOrParent";
    })
    .AddCookie()
    .AddOpenIdConnect("azure", options =>
    {
        options.ClientId = "a92c905b-bb7c-47cd-b317-a4c7e3c0c22a";
        options.Authority = "https://login.microsoftonline.com/277d564c-30a9-4bce-a18d-afc8e54540e5";
        options.ResponseType = OpenIdConnectResponseType.IdToken;
        options.SaveTokens = true;
        options.RequireHttpsMetadata = true;

        options.CallbackPath = "/signin-oidc/azure";
        
        options.Scope.Add("openid");
        options.Scope.Add("profile");

        options.Events.OnRedirectToIdentityProvider = n =>
        {
            var redirect = $"{n.Request.Scheme}://{n.Request.Host}{n.Request.PathBase}{options.CallbackPath}";
            n.ProtocolMessage.RedirectUri = redirect;
            return Task.CompletedTask;
        };
    })
    .AddOpenIdConnect("parent", options =>
    {
        options.ClientId = "b16c7b5452c04c3f9e679bb97010d5e1";
        options.Authority = "https://webappsdev.jefferson.kyschools.us/identity";
        options.ResponseType = OpenIdConnectResponseType.IdToken;
        options.SaveTokens = true;
        options.RequireHttpsMetadata = true;

        options.CallbackPath = "/signin-oidc/parent";
        
        options.Scope.Add("openid");
        options.Scope.Add("profile");

        options.Events.OnRedirectToIdentityProvider = n =>
        {
            var redirect = $"{n.Request.Scheme}://{n.Request.Host}{n.Request.PathBase}{options.CallbackPath}";
            n.ProtocolMessage.RedirectUri = redirect;
            return Task.CompletedTask;
        };
    })
    .AddScheme<JcpsPickerAuthSchemeOptions, JcpsPickerAuthHandler>("picker", options =>
    {
        options.PickAuthenticationUrl = "/Pick";
    })
    .AddPolicyScheme("AzureOrParent", "AzureOrParent", options =>
    {
        options.ForwardDefaultSelector = context =>
            context.Request.Path.StartsWithSegments("/LoginAz") ? "azure"
            : context.Request.Path.StartsWithSegments("/LoginPa") ? "parent"
            : "picker";
    });


var app = builder.Build();

app.UsePathBase("/AzureParentTest");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();