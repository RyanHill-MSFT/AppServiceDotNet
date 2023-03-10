using Azure.Identity;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using webapp.Services;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights.Extensibility;
using webapp.Helper;

var builder = WebApplication.CreateBuilder(args);
var cred = new DefaultAzureCredential(new DefaultAzureCredentialOptions() {
    Diagnostics =
    {
        LoggedHeaderNames = { "x-ms-request-id" },
        LoggedQueryParameters = { "api-version" },
        IsLoggingContentEnabled = true
    }
});

// Configure Azure logging
builder.Logging.AddAzureWebAppDiagnostics();
builder.Services.Configure<AzureFileLoggerOptions>(options => {
    options.FileName = "azure-diagnostics-";
    options.FileSizeLimit = 50 * 1024;
    options.RetainedFileCountLimit = 5;
});

// Add Azure App Configuration to the container.
string? appConfig = builder.Configuration.GetValue<string>("AppConfig");
builder.Configuration.AddAzureAppConfiguration(options => {
    // Use Azure Active Directory authentication.
    // The identity of this app should be assigned 'App Configuration Data Reader' or 'App Configuration Data Owner' role in App Configuration.
    // For more information, please visit https://aka.ms/vs/azure-app-configuration/concept-enable-rbac

    if (Uri.TryCreate(appConfig, UriKind.Absolute, out var endpoint)) {
        options.Connect(endpoint, cred).ConfigureKeyVault(kv => kv.SetCredential(cred));
    }
    else {
        options.Connect(appConfig).ConfigureKeyVault(kv => kv.SetCredential(cred));
    }
});

var initialScopes = builder.Configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ')
                    ?? builder.Configuration.GetValue<string>("MicrosoftGraph:Scopes")?.Split(' ');

// Add services to the container.
builder.Services.AddAzureAppConfiguration()
                .AddApplicationInsightsTelemetry()
                .AddMicrosoftIdentityWebAppAuthentication(builder.Configuration)
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
                .AddInMemoryTokenCaches();

builder.Services.AddRazorPages()
                .AddMicrosoftIdentityUI();

builder.Services.AddSingleton<ITelemetryInitializer, WebAppTelemetryInitializer>()
                .AddTransient<IBufferedFileUpload, BufferedFileUpload>()
                .AddHostedService<BackgroundWorkerService>();

var app = builder.Build();
app.Lifetime.ApplicationStarted.Register(() => app.Logger.LogInformation("Application started"));
app.Lifetime.ApplicationStopping.Register(() => app.Logger.LogInformation("Application stopping"));
app.Lifetime.ApplicationStopped.Register(() => app.Logger.LogInformation("Application stopped"));
app.MapGet("/api/health", () => Results.Ok);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
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
