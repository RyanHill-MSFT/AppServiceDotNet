using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.AppService.Models;
using Azure.ResourceManager.Resources;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Let's get it started HA!!...");
Console.WriteLine("Enter resource group: ");
string? resourceGroupName = Console.ReadLine();

ArmClient client = new ArmClient(new DefaultAzureCredential());
SubscriptionResource subscription = await client.GetDefaultSubscriptionAsync();
ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(resourceGroupName);

Console.WriteLine("Getting app services...");
await foreach (WebSiteResource webSiteResource in resourceGroup.GetWebSites())
{
    Console.WriteLine($"Web site: {webSiteResource.Data}");
}

WebSiteCollection webSites = resourceGroup.GetWebSites();
WebSiteData website = webSites.SingleOrDefault(w => w.Data.Name == "mywebsite")?.Data ?? new WebSiteData(AzureLocation.EastUS);
ArmOperation<WebSiteResource> operation = await webSites.CreateOrUpdateAsync(WaitUntil.Completed, "mywebsite", website);