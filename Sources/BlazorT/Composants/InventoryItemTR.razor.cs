using System;
using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorT.Modals;
using BlazorT.Models;
using BlazorT.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlazorT.Composants
{
    public partial class InventoryItemTR
    {
        [Parameter]
        public int Index { get; set; }

        [Parameter]
        public Item Item { get; set; }

        [Parameter]
        public bool NoDrop { get; set; }

        [Inject]
        private ILogger<Program> _logger { set; get; }

        [CascadingParameter]
        public InventoryComponent Parent { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }
        [Inject]
        public IDataService DataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        /// <summary>
        /// Gère l'événement de glisser-déposer lorsqu'un élément est glissé sur la case.
        /// Ajoute une action d'inventaire pour enregistrer l'événement.
        /// </summary>
        internal void OnDragEnter()
        {
            if (NoDrop)
            {
                _logger.LogWarning("No drop Enter");
                return;
            }
            _logger.LogInformation($"Drag Enter --- <{this.Item.DisplayName}>");
            Parent.Actions.Add(new InventoryAction { Action = "Drag Enter", Item = this.Item, Index = this.Index });
        }

        /// <summary>
        /// Gère l'événement de glisser-déposer lorsqu'un élément est glissé hors de la case.
        /// Ajoute une action d'inventaire pour enregistrer l'événement.
        /// </summary>
        internal void OnDragLeave()
        {
            if (NoDrop)
            {
                _logger.LogWarning("No drop Enter");
                return;
            }
            _logger.LogInformation($"Drag Leave --- <{this.Item.DisplayName}>");

            Parent.Actions.Add(new InventoryAction { Action = "Drag Leave", Item = this.Item, Index = this.Index });
        }

        /// <summary>
        /// Gère l'événement de glisser-déposer lorsqu'un élément est déposé sur la case.
        /// Ajoute une action d'inventaire pour enregistrer l'événement, met à jour l'élément de la case et vérifie si une recette est possible.
        /// </summary>
        internal void OnDrop()
        {
            if (NoDrop)
            {
                _logger.LogWarning("No drop Enter");

                return;
            }

            this.Item = Parent.CurrentDragItem;
            Parent.RecipeItems[this.Index] = this.Item;

            _logger.LogInformation($"Drop --- <{this.Item.DisplayName}>");

            Parent.Actions.Add(new InventoryAction { Action = "Drop", Item = this.Item, Index = this.Index });

            // Check recipe
            Parent.CheckRecipe();
        }

        /// <summary>
        /// Gère l'événement de glisser-déposer lorsqu'un élément est déplacé.
        /// Ajoute une action d'inventaire pour enregistrer l'événement.
        /// </summary>
        private void OnDragStart()
        {
            Parent.CurrentDragItem = this.Item;
            _logger.LogInformation($"Drag started --- <{this.Item.DisplayName}>");

            Parent.Actions.Add(new InventoryAction { Action = "Drag Start", Item = this.Item, Index = this.Index });
        }

        /// <summary>
        /// Gère l'événement de suppression d'un élément.
        /// Affiche une fenêtre modale de confirmation et supprime l'élément s'il est confirmé.
        /// Ajoute une action d'inventaire pour enregistrer l'événement et recharge la page.
        /// </summary>
        /// <param name="id">ID de l'élément à supprimer</param>
        private async Task OnDeleteAsync(int id)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(Item.Id), id);

            var modal = Modal.Show<DeleteConfirmation>("Delete Confirmation", parameters);
            var result = await modal.Result;

            if (result.Cancelled)
            {
                return;
            }

            await DataService.Delete(id);
            _logger.LogInformation($"Item deleted Item id --- <{id}>");

            // Reload the page
            NavigationManager.NavigateTo("inventory", true);
        }


    }
}
