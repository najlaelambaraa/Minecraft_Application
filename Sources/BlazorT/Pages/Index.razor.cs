using System;
using BlazorT.Composants;
using BlazorT.Models;
using BlazorT.Services;
using Microsoft.AspNetCore.Components;
using Sve.Blazor.Core.Services;

namespace BlazorT.Pages
{
    public partial class Index
    {
        [Inject]
        public IDataService DataService { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();

        private List<CraftingRecipe> Recipes { get; set; } = new List<CraftingRecipe>();
        ///<summary>
        ///
        ///  Cette methode charge les données des éléments (Items) à partir de la source de données, ainsi que le nombre de recettes à afficher (NombreRecipes) 
         /// et les recettes elles-mêmes(Recipes), en utilisant des appels asynchrones aux méthodes DataService.
        ///</summary>



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRenderAsync(firstRender);

            if (!firstRender)
            {
                return;
            }

            Items = await DataService.List(0, 12);
            Recipes = await DataService.GetRecipes();

            StateHasChanged();
        }
    }
}

