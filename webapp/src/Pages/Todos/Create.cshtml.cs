using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Models;
using webapp.Services;

namespace webapp.Pages.Todos
{
    public class CreateModel : PageModel
    {
        private readonly ICosmosDbService cosmosDb;

        public CreateModel(ICosmosDbService cosmosDb) => this.cosmosDb = cosmosDb;

        public void OnGet()
        {
            Item = new Item()
            {
                Id = Guid.NewGuid().ToString()
            };
        }

        [BindProperty]
        public Item Item { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await cosmosDb.AddItemAsync(Item);
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
