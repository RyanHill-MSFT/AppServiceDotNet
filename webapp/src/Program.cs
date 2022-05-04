using Azure.Identity;
using Microsoft.Azure.Cosmos;
using System.Text.Json;
using webapp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add app configuration service
builder.Host
    .ConfigureAppConfiguration(config =>
    {
        config.AddAzureAppConfiguration(options =>
        {
            var credential = new DefaultAzureCredential();
            options.Connect((string?)builder.Configuration.GetConnectionString("AppConfig"));
            options.ConfigureKeyVault(options => options.SetCredential(credential));
        });
    })
    .ConfigureLogging(builder =>
    {
        builder.AddJsonConsole(options =>
        {
            options.IncludeScopes = true;
            options.TimestampFormat = "yyyy-MM-dd hh:mm:ss";
            options.JsonWriterOptions = new JsonWriterOptions() { Indented = false };
        });
    });

// Add services to the container.
builder.Services.AddSingleton<ICosmosDbService>(s =>
{
    var section = builder.Configuration.GetSection("CosmosDb");
    string account = section.GetSection("Account").Value;
    string key = section.GetSection("Key").Value;
    string databaseName = section.GetSection("DatabaseName").Value;
    string containerName = section.GetSection("ContainerName").Value;

    CosmosClient client = new(account, key);
    CosmosDbService cosmosDbService = new(client, databaseName, containerName);
    DatabaseResponse database = client.CreateDatabaseIfNotExistsAsync(databaseName).Result;
    database.Database.CreateContainerIfNotExistsAsync(containerName, "/id").Wait();

    return cosmosDbService;
});
builder.Services.AddRazorPages();

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

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
