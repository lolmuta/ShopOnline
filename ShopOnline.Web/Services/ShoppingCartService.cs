using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;
using System.Text;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient httpClient;
        public event Action<int> OnShoppingCarteChanged;

        public ShoppingCartService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/ShoppingCart", cartItemToAddDto);
                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"http status:{response.StatusCode} message:{message}");
                }
                if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(CartItemDto);
                }

                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }
            catch (Exception)
            {
                //todo log
                throw;
            }
        }

        public async Task<CartItemDto> DeleteItem(int id)
        {
            var response = await httpClient.DeleteAsync($"api/ShoppingCart/{id}");
            if(!response.IsSuccessStatusCode)
            {
                return default(CartItemDto);
            }
           
            var readResponse = await response.Content.ReadFromJsonAsync<CartItemDto>();
            return readResponse;
            
        }

        public async Task<List<CartItemDto>> GetItems(int userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");
                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"http status:{response.StatusCode} message:{message}");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<CartItemDto>().ToList();
                }
                return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
            }
            catch (Exception)
            {
                //todo log
                throw;
            }
        }

        public void RaiseEventOnShopppingCartChanged(int total)
        {
            if (OnShoppingCarteChanged != null)
            {
                OnShoppingCarteChanged.Invoke(total);
            }
        }

        public async Task<CartItemDto> UpdateQty(CartItemToUpdateDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PatchAsync($"api/ShoppingCart/{dto.CartItemId}", content);
            if (response.IsSuccessStatusCode)
            {
                var readResponse = await response.Content.ReadFromJsonAsync<CartItemDto>();
                return readResponse;
            }
            else
            {
                //todo why return this... there is a problem to show better
                return default(CartItemDto);
            }
        }
    }
}
