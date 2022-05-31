using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class DisplayProductsBase : ComponentBase
    {
        [Inject] public IProductService ProductService { get; set; }
        [Parameter] public IEnumerable<ProductDto> Products { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //Products = await ProductService.GetItems();
        }
    }
}
