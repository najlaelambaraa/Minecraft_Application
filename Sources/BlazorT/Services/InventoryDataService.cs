using System;
using BlazorT.Composants;
using BlazorT.Models;
using Microsoft.Extensions.Logging;

namespace BlazorT.Services;
public class InventoryDataService : IInventoryDataService
{
    private readonly HttpClient _http;
    private ILogger<InventoryDataService> _logger;


    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryDataService"/> class.
    /// </summary>
    /// <param name="http">The HttpClient used for HTTP requests.</param>
    /// <param name="logger">The ILogger used for logging.</param>
    public InventoryDataService(HttpClient http, ILogger<InventoryDataService> logger)
    {
        _http = http;
        _logger = logger;
    }

    /// <summary>
    /// Adds a new item to the inventory.
    /// </summary>
    /// <param name="model">The item model to add.</param>
    public async Task Add(ItemModel model)
    {
        // Get the item
        var item = ItemFactory.Create(model);

        _logger.LogInformation($"Creating Element with id....... <{item.Name}>");

        // Save the data
        await _http.PostAsJsonAsync("https://localhost:7234/api/Crafting/", item);
    }

    /// <summary>
    /// Retrieves the number of items in the inventory.
    /// </summary>
    /// <returns>The number of items in the inventory.</returns>
    public async Task<int> Count()
    {
        return await _http.GetFromJsonAsync<int>("https://localhost:7234/api/Crafting/count");
    }

    /// <summary>
    /// Retrieves a list of items in the inventory.
    /// </summary>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A list of items in the inventory.</returns>
    public async Task<List<Item>> List(int currentPage, int pageSize)
    {
        _logger.LogInformation($".......List fetching........ <{currentPage}>;<{pageSize}>");
        return await _http.GetFromJsonAsync<List<Item>>($"https://localhost:7234/api/Crafting/?currentPage={currentPage}&pageSize={pageSize}");
    }

    /// <summary>
    /// Retrieves an item in the inventory by its ID.
    /// </summary>
    /// <param name="id">The ID of the item to retrieve.</param>
    /// <returns>The item with the specified ID.</returns>
    public async Task<Item> GetById(int id)
    {
        _logger.LogInformation($"Element with id....... <{id}>");

        return await _http.GetFromJsonAsync<Item>($"https://localhost:7234/api/Crafting/{id}");
    }

    /// <summary>
    /// Updates an item in the inventory.
    /// </summary>
    /// <param name="id">The ID of the item to update.</param>
    /// <param name="model">The updated item model.</param>
    public async Task Update(int id, ItemModel model)
    {
        // Get the item
        var item = ItemFactory.Create(model);
        _logger.LogInformation($"Update ---- Element with id....... <{id}>");

        await _http.PutAsJsonAsync($"https://localhost:7234/api/Crafting/{id}", item);
    }

    /// <summary>
    /// Deletes an item from the inventory.
    /// </summary>
    /// <param name="id">The ID of the item to delete.</param>
    public async Task Delete(int id)
    {
        _logger.LogInformation($"Deleting Element with id....... <{id}>");

        await _http.DeleteAsync($"https://localhost:7234/api/Crafting/{id}");
    }

    /// <summary>
    ///


    public async Task<List<InventoryRecipe>> GetRecipes()
    {
        return await _http.GetFromJsonAsync<List<InventoryRecipe>>("https://localhost:7234/api/Crafting/recipe");
    }
}

