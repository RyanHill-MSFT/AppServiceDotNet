using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Models;
using webapp.Services;

namespace webapp.Pages.Todos
{
    public class DeleteModel : PageModel
    {
        private readonly ICosmosDbService cosmosDb;

        public DeleteModel(ICosmosDbService cosmosDb) => this.cosmosDb = cosmosDb;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var item = await cosmosDb.GetItemAsync(id);
            if(item == null)
            {
                return NotFound();
            }

            Item = item;
            return Page();
        }

        public async void OnPostAsync(string id)
        {
            await cosmosDb.DeleteItemAysnc(id);
            RedirectToAction("Todos/Index");
        }

        [BindProperty]
        public Item Item { get; set; }
    }
}
