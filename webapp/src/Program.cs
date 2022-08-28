using System.Text.Json;
using Azure.Core.Diagnostics;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);
using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();
builder.Logging.AddAzureWebAppDiagnostics();

// Add Azure App Configuration to the container.
builder.Configuration.AddAzureAppConfiguration(options =>
{
    // Use Azure Active Directory authentication.
    // The identity of this app should be assigned 'App Configuration Data Reader' or 'App Configuration Data Owner' role in App Configuration.
    // For more information, please visit https://aka.ms/vs/azure-app-configuration/concept-enable-rbac

    var cred = new DefaultAzureCredential(new DefaultAzureCredentialOptions()
    {
        Diagnostics =
        {
            LoggedHeaderNames = { "x-ms-request-id" },
            LoggedQueryParameters = { "api-version" },
            IsLoggingContentEnabled = true
        }
    });
    string appConfig = builder.Configuration["Endpoints:AppConfig"];
    if (Uri.TryCreate(appConfig, UriKind.Absolute, out var endpoint))
    {
        options.Connect(endpoint, cred).ConfigureKeyVault(kv => kv.SetCredential(cred));
    }
    else
    {
        options.Connect(appConfig).ConfigureKeyVault(kv => kv.SetCredential(cred));
    }
});

var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ')
                    ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');

// Add services to the container.
builder.Services.AddAzureAppConfiguration();

builder.Services.AddRazorPages()
                .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options => options.FallbackPolicy = options.DefaultPolicy)
                .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
                .AddInMemoryTokenCaches();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAzureAppConfiguration();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
