﻿using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetCategories()
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Product/GetCategories");
                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<ProductCategoryDto>();
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();
            }
            catch (Exception)
            {
                //todo log
                throw;
            }
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductDto);
                    }
                    return await response.Content.ReadFromJsonAsync<ProductDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {
                //todo log
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/Product");
                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<ProductDto>();
                }

                return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
            }
            catch (Exception)
            {
                //todo log
                throw;
            }
        }
    }
}
