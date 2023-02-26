using System;
using Microsoft.AspNetCore.Components;

namespace BlazorT.Composants
{
    public partial class ShowItems<TItem>
    {
        [Parameter]
        public List<TItem> Items { get; set; }

        [Parameter]
        public RenderFragment<TItem> ShowTemplate { get; set; }
    }
}

