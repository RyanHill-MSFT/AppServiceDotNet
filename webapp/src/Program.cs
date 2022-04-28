using Azure.Identity;
using Microsoft.Azure.Cosmos;
using webapp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add app configuration service
var connectionString = builder.Configuration.GetConnectionString("AppConfig");
builder.Host.ConfigureAppConfiguration(config =>
{
    config.AddAzureAppConfiguration(options =>
    {
        options.Connect(connectionString);
        options.ConfigureKeyVault(options =>
        {
            options.SetCredential(new DefaultAzureCredential());
        });
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

    CosmosClient client = new CosmosClient(account, key);
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
