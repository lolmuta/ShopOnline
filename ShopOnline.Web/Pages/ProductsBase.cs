using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.GetItems();
            var shoppingCartItems = await ShoppingCartService.GetItems(HardCode.UserId);
            var totalQty = shoppingCartItems.Sum(x => x.Qty);
            ShoppingCartService.RaiseEventOnShopppingCartChanged(totalQty);
        }

        protected IEnumerable<IGrouping<int, ProductDto>> GetGorupProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId into categoryGroup
                   orderby categoryGroup.Key
                   select categoryGroup;
        }
        protected string GetCategoryName(int CategoryId)
        {
            return Products.FirstOrDefault(x => x.CategoryId == CategoryId).CategoryName;
        }
    }
}
