 using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> CartItemDtos { get; set; }
        public string ErrorMessage { get; set; }
        public string TotalPrice { get; private set; }
        public int TotalQuantity { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                CartItemDtos = await ShoppingCartService.GetItems(HardCode.UserId);
                CartChanged();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        protected async Task DeleteCartItem_Click(int id)
        {
            try
            {
                var item = ShoppingCartService.DeleteItem(id);
                removeItemFromClient(id);
                CartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        private void removeItemFromClient(int it)
        {
            var del = CartItemDtos.Where(x => x.Id == it)
                .FirstOrDefault();
            CartItemDtos.Remove(del);
        }
        protected async Task UpdateQty_Click(int id, int qty)
        {
            try
            {
                var item = CartItemDtos.Find(x => x.Id == id);
                if (qty < 0)
                {
                    qty = 1;
                    item.Qty = 1;
                }
                var dto = new CartItemToUpdateDto()
                {
                    CartItemId = id,
                    Qty = qty
                };
                var result = await ShoppingCartService.UpdateQty(dto);
                UpdateItemTotalPrice(result);
                CartChanged();
                await MakeUpdateQtyButtonVisible(id, false);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                throw;
            }
        }
        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if (item != null)
            {
                item.TotoalPrice = item.Qty * item.Price;
            }
        }
        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }
        private void SetTotalPrice()
        {
            TotalPrice = this.CartItemDtos.Sum(x => x.TotoalPrice).ToString("C");
        }
        private void SetTotalQuantity()
        {
            TotalQuantity = this.CartItemDtos.Sum(x => x.Qty);
        }
        private CartItemDto GetCartItem(int id)
        {
            return CartItemDtos.FirstOrDefault(i => i.Id == id);
        }
        protected async Task UpdateQty_Input(int id)
        {
            await MakeUpdateQtyButtonVisible(id, true);
        }
        private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
        }
        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShopppingCartChanged(TotalQuantity);
        }
    }
}
