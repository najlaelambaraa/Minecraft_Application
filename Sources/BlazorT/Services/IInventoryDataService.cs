using BlazorT.Composants;
using BlazorT.Models;

namespace BlazorT.Services
{
    public interface IInventoryDataService
    {
        /// <summary>
        /// Ajoute un élément à l'inventaire.
        /// </summary>
        /// <param name="model">Modèle de l'élément à ajouter.</param>
        public   Task Add(ItemModel model);

        /// <summary>
        /// Récupère le nombre total d'éléments dans l'inventaire.
        /// </summary>
        /// <returns>Le nombre total d'éléments dans l'inventaire.</returns>
        public   Task<int> Count();

        /// <summary>
        /// Récupère une liste d'éléments de l'inventaire.
        /// </summary>
        /// <param name="currentPage">La page courante de la liste.</param>
        /// <param name="pageSize">Le nombre d'éléments par page.</param>
        /// <returns>Une liste d'éléments de l'inventaire.</returns>
        public   Task<List<Item>> List(int currentPage, int pageSize);

        /// <summary>
        /// Récupère un élément de l'inventaire par ID.
        /// </summary>
        /// <param name="id">ID de l'élément à récupérer.</param>
        /// <returns>L'élément de l'inventaire correspondant à l'ID spécifié.</returns>
        public   Task<Item> GetById(int id);

        /// <summary>
        /// Met à jour un élément de l'inventaire.
        /// </summary>
        /// <param name="id">ID de l'élément à mettre à jour.</param>
        /// <param name="model">Modèle de l'élément à mettre à jour.</param>
        public   Task Update(int id, ItemModel model);

        /// <summary>
        /// Supprime un élément de l'inventaire.
        /// </summary>
        /// <param name="id">ID de l'élément à supprimer.</param>
        public   Task Delete(int id);

        /// <summary>
        /// Récupère une liste de recettes d'artisanat.
        /// </summary>
        /// <returns>Une liste de recettes d'artisanat.</returns>
        public   Task<List<InventoryRecipe>> GetRecipes();
    }
}