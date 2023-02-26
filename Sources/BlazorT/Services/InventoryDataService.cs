﻿using System;
using BlazorT.Composants;
using BlazorT.Models;

namespace BlazorT.Services;
public class InventoryDataService : IInventoryDataService
{
    private readonly HttpClient _http;

    public InventoryDataService(
        HttpClient http)
    {
        _http = http;
    }

    public async Task Add(ItemModel model)
    {
        // Get the item
        var item = ItemFactory.Create(model);

        // Save the data
        await _http.PostAsJsonAsync("https://localhost:7234/api/Crafting/", item);
    }

    public async Task<int> Count()
    {
        return await _http.GetFromJsonAsync<int>("https://localhost:7234/api/Crafting/count");
    }

    public async Task<List<Item>> List(int currentPage, int pageSize)
    {
        return await _http.GetFromJsonAsync<List<Item>>($"https://localhost:7234/api/Crafting/?currentPage={currentPage}&pageSize={pageSize}");
    }

    public async Task<Item> GetById(int id)
    {
        return await _http.GetFromJsonAsync<Item>($"https://localhost:7234/api/Crafting/{id}");
    }

    public async Task Update(int id, ItemModel model)
    {
        // Get the item
        var item = ItemFactory.Create(model);

        await _http.PutAsJsonAsync($"https://localhost:7234/api/Crafting/{id}", item);
    }

    public async Task Delete(int id)
    {
        await _http.DeleteAsync($"https://localhost:7234/api/Crafting/{id}");
    }

    public async Task<List<InventoryRecipe>> GetRecipes()
    {
        return await _http.GetFromJsonAsync<List<InventoryRecipe>>("https://localhost:7234/api/Crafting/recipe");
    }
}

