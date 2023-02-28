﻿using System;
using BlazorT.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorT.Composants
{
    public partial class CraftingItem
    {
        [Parameter]
        public int Index { get; set; }


        [Parameter]
        public int Count { get; set; } = 0;

        [Parameter]
        public Item Item { get; set; }

        [Parameter]
        public bool NoDrop { get; set; }

        [CascadingParameter]
        public Crafting Parent { get; set; }

        internal void OnDragEnter()
        {
            if (NoDrop)
            {
                return;
            }

            Parent.Actions.Add(new CraftingAction { Action = "Drag Enter", Item = this.Item, Index = this.Index });
             Count += 1;
        }

        internal void OnDragLeave()
        {
            if (NoDrop)
            {
                return;
            }
            Count = 0;
            Item = null;
            Parent.Actions.Add(new CraftingAction { Action = "Drag Leave", Item = this.Item, Index = this.Index });
        }

        internal void OnDrop()
        {
            if (NoDrop)
            {
                return;
            }

            this.Item = Parent.CurrentDragItem;
            Parent.RecipeItems[this.Index] = this.Item;

            Parent.Actions.Add(new CraftingAction { Action = "Drop", Item = this.Item, Index = this.Index });

            // Check recipe
            Parent.CheckRecipe();
        }

        private void OnDragStart()
        {
            Parent.CurrentDragItem = this.Item;

            Parent.Actions.Add(new CraftingAction { Action = "Drag Start", Item = this.Item, Index = this.Index });
        }
    }
}
