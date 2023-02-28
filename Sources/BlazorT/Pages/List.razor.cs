using System;
using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazorise.DataGrid;
using BlazorT.Modals;
using BlazorT.Models;
using BlazorT.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorT.Pages;

public partial class List
{

    private List<Item> items;


    private List<Etudiant> Etudiants = new List<Etudiant>();


    private int totalItem;

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }

    [Inject]
    public IDataService DataService { get; set; }

    [Inject]
    public IWebHostEnvironment WebHostEnvironment { get; set; }

    [Inject]
    public HttpClient Http { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    public IModalService Modal { get; set; }
    /*  protected override async Task OnInitializedAsync()
      {
          items = await Http.GetFromJsonAsync<Item[]>($"{NavigationManager.BaseUri}fake-data.json");
      }
    */

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // Do not treat this action if is not the first render
        if (!firstRender)
        {
            return;
        }


        Etudiants.Add(new Etudiant { Nom = "Alii", Prenom = "BABABABA" });
        Etudiants.Add(new Etudiant { Nom = "Aii", Prenom = "BBABA" });

        var currentData = await LocalStorage.GetItemAsync<Item[]>("data");

        // Check if data exist in the local storage
        if (currentData == null)
        {
            // this code add in the local storage the fake data (we load the data sync for initialize the data before load the OnReadData method)
            var originalData = Http.GetFromJsonAsync<Item[]>($"{NavigationManager.BaseUri}fake-data.json").Result;

            //Sauvegarde des elements dans le stockage local du navigateur
            await LocalStorage.SetItemAsync("data", originalData);
        }
    }
    /// <summary>
    ///  méthode asynchrone qui gère la suppression d'un élément dans la liste. Elle prend en paramètre l'ID de l'élément à supprimer.
    
    /// </summary>
     

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

        // Reload the page
        NavigationManager.NavigateTo("list", true);
    }

    private async Task OnReadData(DataGridReadDataEventArgs<Item> e)
    {
        if (e.CancellationToken.IsCancellationRequested)
        {
            return;
        }

        // When you use a real API, we use this follow code
        //var response = await Http.GetJsonAsync<Item[]>( $"http://my-api/api/data?page={e.Page}&pageSize={e.PageSize}" );

        // 
     /**/ var response = await LocalStorage.GetItemAsync<Item[]>("data");

        if (!e.CancellationToken.IsCancellationRequested)
        {
            totalItem = response.Count();
            items = new List<Item>(response); // an actual data for the current page
        }

    }

}

