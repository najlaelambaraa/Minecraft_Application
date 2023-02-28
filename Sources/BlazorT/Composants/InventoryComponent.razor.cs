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


        // Fonction permettant de vérifier si la combinaison actuelle d'objets correspond à une recette existante
        // Elle ne retourne rien
        public void CheckRecipe()
        {
            RecipeResult = null;

            // Obtenir le modèle actuel
            var currentModel = string.Join("|", this.RecipeItems.Select(s => s != null ? s.Name : string.Empty));

            // Ajouter l'action dans la liste des actions
            this.Actions.Add(new InventoryAction { Action = $"Items : {currentModel}" });
            _logger.LogInformation("Vérification de la recette.....");

            // Vérifier chaque recette
            foreach (var inventoryRecipe in Recipes)
            {
                // Obtenir le modèle de la recette
                var recipeModel = string.Join("|", inventoryRecipe.Have.SelectMany(s => s));

                // Ajouter l'action dans la liste des actions
                this.Actions.Add(new InventoryAction { Action = $"Modèle de la recette : {recipeModel}" });

                // Si le modèle actuel correspond au modèle de la recette, attribuer la valeur Give de la recette à RecipeResult
                if (currentModel == recipeModel)
                {
                    RecipeResult = inventoryRecipe.Give;
                }
            }
        }




        /// <summary>
        /// Override method that is called whenever the component's parameters are updated.
        /// Sets the items list to the current Items property and initializes the RecipeItems list with null items.
        /// Calls the onSearching method to filter the items based on the search value.
        /// Logs a message indicating that the parameters have been set.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            items = Items;
            this.RecipeItems = Enumerable.Repeat<Item>(null, NombreRecipes).ToList();
            this.onSearching("");
            _logger.LogInformation("Parameters set: Items, RecipeItems");
        }


        /// <summary>
        /// Event handler for searching. Filters the items list based on the search value and updates the UI.
        /// </summary>
        /// <param name="e">The search string.</param>
        private void onSearching(string e)
        {
            // Set the search value and reset the page to 1.
            searchValue = e;
            _customFilterRef.Page = 1;

            // If the search string is not empty, filter the items list by name or display name and log a message.
            if (!string.IsNullOrEmpty(searchValue))
            {
                Items = items.Where(q => q.Name.ToLower().Contains(searchValue.ToLower()) || q.DisplayName.ToLower().Contains(searchValue.ToLower())).ToList();
                _logger.LogInformation($"Searching... {e}");
            }
            // If the search string is empty, retrieve the first 20 items and log a message.
            else
            {
                Items = items.Take(20).ToList();
                _logger.LogInformation("Fetching All ...");
            }

            // Notify the UI that the state has changed.
            StateHasChanged();
        }



        /// <summary>
        /// Event handler for a collection changed event. Invokes a JavaScript function to add new items to the inventory.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The collection changed event arguments.</param>
        private void OnActionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Invoke a JavaScript function to add the new items to the inventory.
            JavaScriptRuntime.InvokeVoidAsync("Inventory.AddActions", e.NewItems);
        }



        /// <summary>
        /// Récupère une liste d'objets Item à partir d'une demande de données en fonction des paramètres de filtrage et de tri spécifiés.
        /// </summary>
        /// <param name="dataRequest">L'objet DataRequest contenant les paramètres de filtrage et de tri.</param>
        /// <returns>Un tuple contenant une liste d'objets Item et un entier représentant le nombre total d'objets.</returns>
        private async Task<(IEnumerable<Item>, int)> FetchItems(DataRequest dataRequest)
        {
            // Initialisation du compteur avec la taille actuelle de la liste d'objets items
            var count = items.Count;// Vérification si les propriétés de filtrage sont spécifiées
            if (dataRequest.FilterColumnProperty != null && dataRequest.Filter != null)
            {
                // Filtrer la liste d'objets Items pour obtenir tous les objets correspondant aux paramètres de filtrage spécifiés
                var data = Items.Where(q =>
                    (q.Name.ToLower().Contains(dataRequest.Filter) && nameof(q.Name) == dataRequest.FilterColumn)
                    ).ToList();

                // Mise à jour du compteur avec le nombre total d'objets filtrés
                count = data.Count();

                // Enregistrement d'un message d'information dans le journal des événements
                _logger.LogInformation($"Fetching for {dataRequest.FilterColumnProperty} - {dataRequest.Filter}");

                // Retourne une liste d'objets Item filtrés ainsi que leur nombre total
                return (data, count);
            }

            // Vérification si la propriété de tri est spécifiée
            if (dataRequest.SortColumnProperty != null)
            {
                // Vérification si le tri doit être effectué dans l'ordre décroissant
                if (dataRequest.Descending)
                {
                    // Enregistrement d'un message d'information dans le journal des événements
                    _logger.LogInformation($"Column Sort : {dataRequest.SortColumnProperty} - Descending");

                    // Retourne une liste d'objets Item triés dans l'ordre décroissant ainsi que leur nombre total, limités à la page spécifiée par DataRequest
                    return (Items.OrderByDescending(x => dataRequest.SortColumnProperty.GetValue(x)).Skip(dataRequest.Page * 20).Take(20).ToList(), count);

                }
                else // Le tri doit être effectué dans l'ordre croissant
                {
                    // Enregistrement d'un message d'information dans le journal des événements
                    _logger.LogInformation($"Column Sort : {dataRequest.SortColumnProperty} - Ascending");

                    // Retourne une liste d'objets Item triés dans l'ordre croissant ainsi que leur nombre total, limités à la page spécifiée par DataRequest
                    return (Items.OrderBy(x => dataRequest.SortColumnProperty.GetValue(x)).Skip(dataRequest.Page * 20).Take(20).ToList(), count);
                }
            }

            // Aucune propriété de filtrage ou de tri n'est spécifiée, renvoie simplement une page de 20 objets de la liste Items.
            _logger.LogInformation("Fetching or Sort : Empty");
            return (Items.Skip(dataRequest.Page * 20).Take(20).ToList(), count);


        }
    }

}