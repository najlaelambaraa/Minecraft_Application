using System;
using System.Net;
using System.Runtime.ConstrainedExecution;
using Blazorise;
using System.Text;
using BlazorT.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Routing;
using static System.Collections.Specialized.BitVector32;

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
        ///<summary>
        ///  Cette méthode est appelée lorsque l'utilisateur commence à faire glisser un élément
        /// sur cette zone de dépôt(drop zone).
         
         ///Si la propriété NoDrop est définie à true, cette méthode ne fait rien.
         /// Sinon, elle ajoute une nouvelle action de crafting dans la liste d'actions de l'objet Parent
         //en utilisant les propriétés Item et Index de cet objet, et incrémente la variable Count de 1.
        ///</summary>
        ///

         

        internal void OnDragEnter()
        {
            if (NoDrop)
            {
                // Si la propriété NoDrop est définie à true, ne rien faire.
                return;

            }
            // Ajouter une nouvelle action de crafting dans la liste d'actions de l'objet Parent
            Parent.Actions.Add(new CraftingAction { Action = "Drag Enter", Item = this.Item, Index = this.Index });
            // Incrémenter la variable Count de 1.
            Count += 1;
        }
        ///<summary>
        ///
        /// Cette méthode est appelée lorsque l'utilisateur arrête de faire glisser un élément sur cette zone de dépôt (drop zone)
        /// Si la propriété NoDrop est définie à true, cette méthode ne fait rien.
        ///Sinon, elle réinitialise les propriétés Item et Count de cet objet, et ajoute une nouvelle action de crafting
        /// dans la liste d'actions de l'objet Parent en utilisant les propriétés Item et Index de cet objet.
        /// </summary>
        internal void OnDragLeave()
        {
            if (NoDrop)
            {
                // Si la propriété NoDrop est définie à true, ne rien faire.
                return;
            }
            // Réinitialiser les propriétés Item et Count de cet objet.
            Count = 0;
            Item = null;
            // Ajouter une nouvelle action de crafting dans la liste d'actions de l'objet Parent
            Parent.Actions.Add(new CraftingAction { Action = "Drag Leave", Item = this.Item, Index = this.Index });
        }
        ///<summary>
        ///Cette méthode est appelée lorsque l'utilisateur lâche un élément sur cette zone de dépôt (drop zone).
        ///Si la propriété NoDrop est définie à true, cette méthode ne fait rien.
        ///Sinon, elle définit la propriété Item de cet objet avec l'élément actuellement en train d'être glissé-déposé,
        ///met à jour la liste Parent.RecipeItems avec cet élément à l'index correspondant, ajoute une nouvelle action de crafting
        ///dans la liste d'actions de l'objet Parent en utilisant les propriétés Item et Index de cet objet, et vérifie si la recette est complète.
       
        /// </summary>
       
        internal void OnDrop()
        {
            if (NoDrop)
            {
                // Si la propriété NoDrop est définie à true, ne rien faire.
                return;
            }
            // Définir la propriété Item de cet objet avec l'élément actuellement en train d'être glissé-déposé.
            this.Item = Parent.CurrentDragItem;
            // Mettre à jour la liste Parent.RecipeItems avec cet élément à l'index correspondant.
            Parent.RecipeItems[this.Index] = this.Item;
            
            // Ajouter une nouvelle action de crafting dans la liste d'actions de l'objet Parent.
            Parent.Actions.Add(new CraftingAction { Action = "Drop", Item = this.Item, Index = this.Index });

            // Check recipe
            // Vérifier si la recette est complète.
            Parent.CheckRecipe();
        }
        ///<summary>
        /// Cette méthode est appelée lorsque l'utilisateur commence à faire glisser un élément de cette zone de dépôt (drop zone).
        /// Elle définit la propriété Parent.CurrentDragItem avec l'élément actuellement en train d'être glissé-déposé,
        ///et ajoute une nouvelle action de crafting dans la liste d'actions de l'objet Parent en utilisant les propriétés Item et Index de cet objet.
        ///
        ///
        /// </summary>




        private void OnDragStart()
        {
            // Définir la propriété Parent.CurrentDragItem avec l'élément actuellement en train d'être glissé-déposé.
            Parent.CurrentDragItem = this.Item;
            // Ajouter une nouvelle action de crafting dans la liste d'actions de l'objet Parent.
            Parent.Actions.Add(new CraftingAction { Action = "Drag Start", Item = this.Item, Index = this.Index });
        }
    }
}

