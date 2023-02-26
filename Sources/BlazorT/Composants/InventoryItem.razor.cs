using System;
using BlazorT.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorT.Composants
{
    public partial class InventoryItem
    {
        [Parameter]
        public int Index { get; set; }

        [Parameter]
        public Item Item { get; set; }
        [Parameter]
        public int Count { get; set; } = 0;

        [Parameter]
        public bool NoDrop { get; set; }

        [CascadingParameter]
        public InventoryComponent Parent { get; set; }

        internal void OnDragEnter()
        {
            if (NoDrop)
            {
                return;
            }
            
            Parent.Actions.Add(new InventoryAction { Action = "Drag Enter", Item = this.Item, Index = this.Index });
        }

        internal void OnDragLeave()
        {
            if (NoDrop)
            {
                return;
            }
            Count = 0;
            Item = null;
            Parent.Actions.Add(new InventoryAction { Action = "Drag Leave", Item = this.Item, Index = this.Index });
        }

        internal void OnDrop()
        {
            if (NoDrop)
            {
                return;
            }

            if(this.Item != null && this.Item.Name == Parent.CurrentDragItem.Name )
            {
                Count += 1;
            }
            else if(this.Item == null)
            {
                this.Item = Parent.CurrentDragItem;
                Parent.RecipeItems[this.Index] = this.Item;
                Count = 1;
            }

            
            
            Parent.Actions.Add(new InventoryAction { Action = "Drop", Item = this.Item, Index = this.Index });

            // Check recipe
            Parent.CheckRecipe();
        }

        private void OnDragStart()
        {
            Parent.CurrentDragItem = this.Item;

            Parent.Actions.Add(new InventoryAction { Action = "Drag Start", Item = this.Item, Index = this.Index });
        }
    }
}

