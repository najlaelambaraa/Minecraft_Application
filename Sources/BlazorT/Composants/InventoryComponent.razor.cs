using System;
using BlazorT.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Blazorise.DataGrid;

namespace BlazorT.Composants
{
    public partial class InventoryComponent
    {
        private Item _recipeResult;

        public InventoryComponent()
        {
            Actions = new ObservableCollection<InventoryAction>();
            Actions.CollectionChanged += OnActionsCollectionChanged;
            

        }

        private DataGrid<Item> gridTableRef;



        private string searchValue;
        public ObservableCollection<InventoryAction> Actions { get; set; }
        public Item CurrentDragItem { get; set; }

        [Parameter]
        public List<Item> Items { get; set; }

        [Parameter]
        public int NombreRecipes { get; set; }

        private List<Item> items;

        public List<Item> RecipeItems { get; set; }

        public Item RecipeResult
        {
            get => this._recipeResult;
            set
            {
                if (this._recipeResult == value)
                {
                    return;
                }

                this._recipeResult = value;
                this.StateHasChanged();
            }
        }

        [Parameter]
        public List<InventoryRecipe> Recipes { get; set; }

        /// <summary>
        /// Gets or sets the java script runtime.
        /// </summary>
        [Inject]
        internal IJSRuntime JavaScriptRuntime { get; set; }

        public void CheckRecipe()
        {
            RecipeResult = null;

            // Get the current model
            var currentModel = string.Join("|", this.RecipeItems.Select(s => s != null ? s.Name : string.Empty));

            this.Actions.Add(new InventoryAction { Action = $"Items : {currentModel}" });

            foreach (var inventoryRecipe in Recipes)
            {
                // Get the recipe model
                var recipeModel = string.Join("|", inventoryRecipe.Have.SelectMany(s => s));

                this.Actions.Add(new InventoryAction { Action = $"Recipe model : {recipeModel}" });

                if (currentModel == recipeModel)
                {
                    RecipeResult = inventoryRecipe.Give;
                }
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            items = Items;
            this.RecipeItems = Enumerable.Repeat<Item>(null, NombreRecipes).ToList();
        }

        private void onSearching(ChangeEventArgs e)
        {
            searchValue = e.Value.ToString();

            Items =  !string.IsNullOrWhiteSpace(searchValue)  ? Items.Where(x => x.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList() : items;
            this.StateHasChanged();
        }

        private void OnActionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            JavaScriptRuntime.InvokeVoidAsync("Inventory.AddActions", e.NewItems);
        }
    }
}

