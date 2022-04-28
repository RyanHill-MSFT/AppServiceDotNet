using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using webapp.Models;
using webapp.Services;

namespace webapp.Pages.Todos
{
    public class EditModel : PageModel
    {
        private readonly ICosmosDbService cosmosDb;

        public EditModel(ICosmosDbService cosmosDb)
        {
            this.cosmosDb = cosmosDb;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            Item = await cosmosDb.GetItemAsync(id);
            if(Item == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (ModelState.IsValid)
            {
                await cosmosDb.UpdateItemAsync(id, Item);
                RedirectToAction("/Todos/Index");
            }

            return Page();
        }

        [BindProperty]
        public Item Item { get; set; }
    }
}
