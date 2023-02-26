using System;
using BlazorT.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorT.Composants
{
	public partial class Card<TItem>
    {
      

        [Parameter]
        public RenderFragment CardFooter { get; set; }

        
        [Parameter]
        public RenderFragment<TItem> CardBody { get; set; }

        [Parameter]
        public RenderFragment CardHeader { get; set; }


        [Parameter]
        public TItem Item { set; get; } 
    }
}

