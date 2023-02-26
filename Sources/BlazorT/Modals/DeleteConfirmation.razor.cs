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

        protected override async Task OnInitializedAsync()
        {
            // Get the item
            item = await DataService.GetById(Id);
        }

        void ConfirmDelete()
        {
            ModalInstance.CloseAsync(ModalResult.Ok(true));
        }

        void Cancel()
        {
            ModalInstance.CancelAsync();
        }
    }
}

