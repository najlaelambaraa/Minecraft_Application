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

        private void OnDragStart()
        {
            Parent.CurrentDragItem = this.Item;
            _logger.LogInformation($"Drag started --- <{this.Item.DisplayName}>");

            Parent.Actions.Add(new InventoryAction { Action = "Drag Start", Item = this.Item, Index = this.Index });
        }


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

