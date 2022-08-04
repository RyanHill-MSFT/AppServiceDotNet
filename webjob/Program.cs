using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = new HostBuilder();
builder
    .UseEnvironment(EnvironmentName.Development)
    .ConfigureWebJobs(b =>
    {
        b.AddAzureStorageCoreServices();
        b.AddAzureStorageQueues();
    })
    .ConfigureLogging((context, b) =>
    {
        b.AddConsole();

        // If the key exists in settings, use it to enable Application Insights.
        string instrumentationKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
        if (!string.IsNullOrEmpty(instrumentationKey))
        {
            b.AddApplicationInsightsWebJobs(o => o.InstrumentationKey = instrumentationKey);
        }
    });

var host = builder.Build();
using (host)
{
    await host.RunAsync();
}