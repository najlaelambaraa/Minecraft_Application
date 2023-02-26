using System;
using BlazorT.Models;

namespace BlazorT.Composants
{
    public class InventoryRecipe
    {
        public Item Give { get; set; }
        public List<List<string>> Have { get; set; }
    }
}

