using BlazorT.Models;

public static class ItemFactory
{
    ///<summary>
    /// Cette méthode statique permet de créer un modèle d'élément (ItemModel) à partir d'une instance de la classe Item.
    
    ///@param item : l'instance de la classe Item utilisée pour créer le modèle d'élément.
    /// @param imageContent : le contenu de l'image de l'élément en tant que tableau d'octets.
    /// @return : un nouveau modèle d'élément avec les propriétés initialisées à partir de l'instance de la classe Item.
    ///</summary>
    public static ItemModel ToModel(Item item, byte[] imageContent)
    {
        // Créer un nouveau modèle d'élément avec les propriétés initialisées à partir de l'instance de la classe Item.
        return new ItemModel
        {
            Id = item.Id,
            DisplayName = item.DisplayName,
            Name = item.Name,
            RepairWith = item.RepairWith,
            EnchantCategories = item.EnchantCategories,
            MaxDurability = item.MaxDurability,
            StackSize = item.StackSize,
            ImageContent = imageContent,
            ImageBase64 = string.IsNullOrWhiteSpace(item.ImageBase64) ? Convert.ToBase64String(imageContent) : item.ImageBase64
        };
    }
    ///<summary>
    /// Cette méthode statique permet de créer une nouvelle instance de la classe Item à partir d'un modèle d'élément (ItemModel).

    ///@param model : le modèle d'élément utilisé pour créer la nouvelle instance de la classe Item.
    /// @return : une nouvelle instance de la classe Item avec les propriétés initialisées à partir du modèle d'élément.
    ///</summary>
    public static Item Create(ItemModel model)
    {
        // Créer une nouvelle instance de la classe Item avec les propriétés initialisées à partir du modèle d'élément.
        return new Item
        {
            Id = model.Id,
            DisplayName = model.DisplayName,
            Name = model.Name,
            RepairWith = model.RepairWith,
            EnchantCategories = model.EnchantCategories,
            MaxDurability = model.MaxDurability,
            StackSize = model.StackSize,
            CreatedDate = DateTime.Now,
            ImageBase64 = Convert.ToBase64String(model.ImageContent)
        };
    }

    public static void Update(Item item, ItemModel model)
    {
        item.DisplayName = model.DisplayName;
        item.Name = model.Name;
        item.RepairWith = model.RepairWith;
        item.EnchantCategories = model.EnchantCategories;
        item.MaxDurability = model.MaxDurability;
        item.StackSize = model.StackSize;
        item.UpdatedDate = DateTime.Now;
        item.ImageBase64 = Convert.ToBase64String(model.ImageContent);
    }
}