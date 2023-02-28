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

        /// <summary>
        /// Gère l'événement de glisser-déposer d'un élément sur la case de l'inventaire.
        /// </summary>
        internal void OnDrop()
        {
            if (NoDrop)
            {
                return;
            }

            // Incrémente le compteur si l'élément glissé correspond à celui déjà présent dans la case.
            if (this.Item != null && this.Item.Name == Parent.CurrentDragItem.Name)
            {
                Count += 1;
            }
            // Ajoute l'élément glissé dans la case si celle-ci est vide.
            else if (this.Item == null)
            {
                this.Item = Parent.CurrentDragItem;
                Parent.RecipeItems[this.Index] = this.Item;
                Count = 1;
            }

            // Ajoute une action à l'historique pour signaler le glisser-déposer.
            Parent.Actions.Add(new InventoryAction { Action = "Drop", Item = this.Item, Index = this.Index });

            // Vérifie si une recette correspond à la combinaison d'éléments.
            Parent.CheckRecipe();
        }

        /// <summary>
        /// Gère l'événement de glisser-déposer d'un élément entrant dans la zone de la case de l'inventaire.
        /// </summary>
        internal void OnDragEnter()
        {
            if (NoDrop)
            {
                return;
            }

            // Ajoute une action à l'historique pour signaler l'entrée d'un élément.
            Parent.Actions.Add(new InventoryAction { Action = "Drag Enter", Item = this.Item, Index = this.Index });
        }

        /// <summary>
        /// Gère l'événement de glisser-déposer d'un élément sortant de la zone de la case de l'inventaire.
        /// </summary>
        internal void OnDragLeave()
        {
            if (NoDrop)
            {
                return;
            }

            // Réinitialise la case si l'élément glissé en sort.
            Count = 0;
            Item = null;

            // Ajoute une action à l'historique pour signaler la sortie de l'élément.
            Parent.Actions.Add(new InventoryAction { Action = "Drag Leave", Item = this.Item, Index = this.Index });
        }

        /// <summary>
        /// Gère l'événement de démarrage d'un glisser-déposer d'un élément sur la case de l'inventaire.
        /// </summary>
        private void OnDragStart()
        {
            // Stocke l'élément en cours de glisser-déposer dans l'inventaire parent.
            Parent.CurrentDragItem = this.Item;

            // Ajoute une action à l'historique pour signaler le début du glisser-déposer.
            Parent.Actions.Add(new InventoryAction { Action = "Drag Start", Item = this.Item, Index = this.Index });
        }

    }
}

