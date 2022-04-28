using webapp.Models;

namespace webapp.Services
{
    public interface ICosmosDbService
    {
        Task AddItemAsync(Item item);

        Task DeleteItemAysnc(string id);

        Task<Item?> GetItemAsync(string id);

        Task<IEnumerable<Item>> GetItemsAsync(string queryString);

        Task UpdateItemAsync(string id, Item item);
    }
}