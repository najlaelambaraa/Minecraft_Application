using System;
using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorT.Models;
using BlazorT.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorT.Modals
{
	public partial class DeleteConfirmation
	{
        [CascadingParameter]
        public BlazoredModalInstance ModalInstance { get; set; }

        [Inject]
        public IDataService DataService { get; set; }

        [Parameter]
        public int Id { get; set; }

        private Item item = new Item();
        ///<summary>
        ///
        ///Elle récupère un élément à partir d'un service de données en utilisant un ID fourni.
        /// </summary>méthode asynchrone qui est appelée lorsque le composant est initialisé. 

        protected override async Task OnInitializedAsync()
        {
            // Get the item
            item = await DataService.GetById(Id);
        }
        ///<summary>
        ///méthode de gestionnaire d'événements pour des actions utilisateur. 
        ///
        ///"ConfirmDelete" est appelé lorsque l'utilisateur confirme qu'il veut supprimer l'élément
        /// </summary>
         
        void ConfirmDelete()
        {
            ModalInstance.CloseAsync(ModalResult.Ok(true));
        }
        ///<summary>
        ///
        ///"Cancel" est appelé lorsque l'utilisateur annule l'opération de suppression et ferme la fenêtre de dialogue modale sans résultat.
        /// </summary>
       
        void Cancel()
        {
            ModalInstance.CancelAsync();
        }
    }
}

