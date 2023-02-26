using BlazorT.Composants;
using BlazorT.Models;

namespace BlazorT.Services
{
    public interface IInventoryDataService
    {
        Task Add(ItemModel model);

        Task<int> Count();

        Task<List<Item>> List(int currentPage, int pageSize);

        Task<Item> GetById(int id);

        Task Update(int id, ItemModel model);

        Task Delete(int id);

        Task<List<InventoryRecipe>> GetRecipes();
    }
}