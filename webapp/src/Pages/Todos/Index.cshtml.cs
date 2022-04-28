using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Models;
using webapp.Services;

namespace webapp.Pages.Todos
{
    public class IndexModel : PageModel
    {
        private readonly ICosmosDbService cosmosDb;

        public IndexModel(ICosmosDbService cosmosDb) => this.cosmosDb = cosmosDb;

        public async Task OnGet()
        {
            Todos = await cosmosDb.GetItemsAsync(CosmosDbServiceConstants.GetItemsQueryString);
        }

        public IEnumerable<Item> Todos { get; set; }
    }
}
