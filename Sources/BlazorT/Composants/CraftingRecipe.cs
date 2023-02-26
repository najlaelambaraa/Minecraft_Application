using System;
using BlazorT.Models;

namespace BlazorT.Composants
{
    public class CraftingRecipe
    {
        public Item Give { get; set; }
        public List<List<string>> Have { get; set; }
    }
}

