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
        ///<summary>
        ///Cette méthode est appelée lorsque l'utilisateur commence à faire glisser un élément sur cette zone de dépôt (drop zone).
        ///Si la propriété NoDrop est définie à true, cette méthode ne fait rien.
        ///Sinon, elle ajoute une nouvelle action d'inventaire dans la liste d'actions de l'objet Parent en utilisant les propriétés Item et Index de cet objet.
        ///
        /// </summary>
       
        internal void OnDragEnter()
        {
            if (NoDrop)
            {
                return;

                // Si la propriété NoDrop est définie à true, ne rien faire.
            }
            // Ajouter une nouvelle action d'inventaire dans la liste d'actions de l'objet Parent en utilisant les propriétés Item et Index de cet objet.
            Parent.Actions.Add(new InventoryAction { Action = "Drag Enter", Item = this.Item, Index = this.Index });
        }
        ///<summary>

        /// Cette méthode est appelée lorsque l'utilisateur arrête de faire glisser un élément sur cette zone de dépôt (drop zone).
        ///
        /// Si la propriété NoDrop est définie à true, cette méthode ne fait rien.
        ///Sinon, elle réinitialise la propriété Count à 0 et la propriété Item à null, puis ajoute une nouvelle action d'inventaire dans la liste d'actions de l'objet Parent en utilisant les propriétés Item et Index de cet objet.
        ///</summary>
        internal void OnDragLeave()
        {
            if (NoDrop)
            {
                // Si la propriété NoDrop est définie à true, ne rien faire.
                return;
            }
            // Réinitialiser la propriété Count à 0 et la propriété Item à null
            Count = 0;
            Item = null;
            // Ajouter une nouvelle action d'inventaire dans la liste d'actions de l'objet Parent en utilisant les propriétés Item et Index de cet objet.
            Parent.Actions.Add(new InventoryAction { Action = "Drag Leave", Item = this.Item, Index = this.Index });
        }

        internal void OnDrop()
        {
            if (NoDrop)
            {
                // Si la propriété NoDrop est définie à true, ne rien faire.
                return;
            }

            if(this.Item != null && this.Item.Name == Parent.CurrentDragItem.Name )
            {
                // Si un élément est déjà présent dans cette zone de dépôt et que c'est le même que celui que l'utilisateur est en train de glisser, incrémenter la propriété Count de cet objet.
                Count += 1;
            }
            else if(this.Item == null)
            {
                // Si aucun élément n'est présent dans cette zone de dépôt, définir la propriété Item de cet objet à l'élément en cours de glissement et modifier la propriété Parent.RecipeItems à l'index de cet objet en utilisant la propriété Item de cet objet. Définir également la propriété Count de cet objet à 1.
                this.Item = Parent.CurrentDragItem;
                Parent.RecipeItems[this.Index] = this.Item;
                Count = 1;
            }


            // Ajouter une nouvelle action d'inventaire dans la liste d'actions de l'objet Parent en utilisant les propriétés Item et Index de cet objet.
            Parent.Actions.Add(new InventoryAction { Action = "Drop", Item = this.Item, Index = this.Index });

            // Vérifier la recette
            Parent.CheckRecipe();
        }
        ///<summary>
        ///Cette méthode est appelée lorsque l'utilisateur commence à glisser un élément dans cette zone de dépôt (drop zone).
        ///Elle définit la propriété Parent.CurrentDragItem à la propriété Item de cet objet.
        ///Elle ajoute ensuite une nouvelle action d'inventaire dans la liste d'actions de l'objet Parent en utilisant les propriétés Item et Index de cet objet.
        /// </summary>
    
        private void OnDragStart()
        {
            // Définir la propriété Parent.CurrentDragItem à la propriété Item de cet objet.
            Parent.CurrentDragItem = this.Item;
            // Ajouter une nouvelle action d'inventaire dans la liste d'actions de l'objet Parent en utilisant les propriétés Item et Index de cet objet.
            Parent.Actions.Add(new InventoryAction { Action = "Drag Start", Item = this.Item, Index = this.Index });
        }
    }
}

