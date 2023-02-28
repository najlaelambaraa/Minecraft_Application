using System;
using System.Security.AccessControl;
using BlazorT.Composants;
using BlazorT.Models;
using BlazorT.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorT.Pages;
public partial class Inventory
{
    [Inject]
    public IInventoryDataService DataService { get; set; }

    public List<Item> Items { get; set; } = new List<Item>();

    private List<InventoryRecipe> Recipes { get; set; } = new List<InventoryRecipe>();

    public int NombreRecipes;

    [Inject]
    public IConfiguration Configuration { set; get; }

    ///<summary>
    /// une méthode asynchrone de cycle de vie appelée OnAfterRenderAsync, qui est exécutée après le rendu initial de la page ou du composant. 
     /// La méthode commence par appeler la version de la méthode de cycle de vie parente avec l'instruction base.OnAfterRenderAsync (firstRender)
     
    ///</summary>
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
        {
            return;
        }

        Items = await DataService.List(0, await DataService.Count());

        NombreRecipes = Configuration.GetValue<int>("NombreRecipes");

        Recipes = await DataService.GetRecipes();

        StateHasChanged();
    }
}