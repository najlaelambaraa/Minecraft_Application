using System;
using BlazorT.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Blazorise.DataGrid;
using BlazorStrap;
using BlazorStrap.V5;
using Blazored.Modal;
using BlazorT.Modals;
using Sve.Blazor.Core.Services;
using Blazored.Modal.Services;
using BlazorT.Services;
using Blazorise;

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

        private BSDataTable<Item> _customFilterRef = new BSDataTable<Item>();

        [Inject]
        private ILogger<Program> _logger { set; get; }

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

       

        protected override void OnInitialized()
        {
            base.OnInitialized();
            StateHasChanged();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            
        }

        public void CheckRecipe()
        {
            RecipeResult = null;

            // Get the current model
            var currentModel = string.Join("|", this.RecipeItems.Select(s => s != null ? s.Name : string.Empty));

            this.Actions.Add(new InventoryAction { Action = $"Items : {currentModel}" });
            _logger.LogInformation("Recipe checking.....");
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
            this.onSearching("");
            _logger.LogInformation("Parameters set: Items, RecipeItems");
        }

        private void onSearching(string e)
        {
            searchValue = e;
            _customFilterRef.Page = 1;
            if (!string.IsNullOrEmpty(searchValue))
            {
                Items = items.Where(q => q.Name.ToLower().Contains(searchValue.ToLower()) || q.DisplayName.ToLower().Contains(searchValue.ToLower())).ToList();
                _logger.LogInformation($"Searching... {e}");
            }
            else
            {
                Items = items.Take(20).ToList();
                _logger.LogInformation("Fetching All ...");
            }
            StateHasChanged();
        }

      
        private void OnActionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            JavaScriptRuntime.InvokeVoidAsync("Inventory.AddActions", e.NewItems);
        }

    

        private async Task<(IEnumerable<Item>, int)> FetchItems(DataRequest dataRequest)
        {
            var count = items.Count;
            if (dataRequest.FilterColumnProperty != null && dataRequest.Filter != null)
            {
                
                var data = Items.Where(q =>
                    (q.Name.ToLower().Contains(dataRequest.Filter) && nameof(q.Name) == dataRequest.FilterColumn) 
                    ).ToList();
                count = data.Count();
                _logger.LogInformation($"Fetching for {dataRequest.FilterColumnProperty} - {dataRequest.Filter}");
                return (data, count);
            }
            if (dataRequest.SortColumnProperty != null)
            {
                if (dataRequest.Descending)
                {
                    _logger.LogInformation($"Column Sort : {dataRequest.SortColumnProperty} - Descending");

                    return (Items.OrderByDescending(x => dataRequest.SortColumnProperty.GetValue(x)).Skip(dataRequest.Page * 20).Take(20).ToList(), count);

                }

                _logger.LogInformation($"Column Sort : {dataRequest.SortColumnProperty} - Ascending");

                return (Items.OrderBy(x => dataRequest.SortColumnProperty.GetValue(x)).Skip(dataRequest.Page * 20).Take(20).ToList(), count);
            }
            _logger.LogInformation("Fetching or Sort : Empty");
            return (Items.Skip(dataRequest.Page * 20).Take(20).ToList(), count);
        }

    }
}

