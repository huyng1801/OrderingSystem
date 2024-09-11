
using Microsoft.AspNetCore.Http;
using OrderingSystemCustomerDTO;

using System.Net.Http.Json;

namespace OrderingSystemCustomerService
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private string BaseUrl = $"{Utils.BaseAddress}/Product";

        public ProductService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<ProductDTO>> GetAllProducts()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ProductDTO>>(BaseUrl);
            return response;
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            var response = await _httpClient.GetFromJsonAsync<ProductDTO>(url);
            return response;
        }
        public async Task<List<ProductDTO>> GetProductsByCategory(int categoryId)
        {
            var url = $"{BaseUrl}/category/{categoryId}";
            var response = await _httpClient.GetFromJsonAsync<List<ProductDTO>>(url);
            return response;
        }

        public async Task<ProductDTO> AddProduct(ProductDTO productDTO, IFormFile imageFile)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(productDTO.ProductName), "productName");
            formData.Add(new StringContent(productDTO.Description), "description");
            formData.Add(new StringContent(productDTO.Price.ToString()), "price");
            formData.Add(new StreamContent(imageFile.OpenReadStream()), "image", imageFile.FileName);

            var response = await _httpClient.PostAsync($"{BaseUrl}/add", formData);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductDTO>();
        }

        public async Task<ProductDTO> UpdateProduct(int id, ProductDTO productDTO, IFormFile imageFile = null)
        {
            var url = $"{BaseUrl}/update/{id}";

            if (imageFile != null)
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(productDTO.ProductName), "productName");
                formData.Add(new StringContent(productDTO.Description), "description");
                formData.Add(new StringContent(productDTO.Price.ToString()), "price");
                formData.Add(new StreamContent(imageFile.OpenReadStream()), "image", imageFile.FileName);

                var response = await _httpClient.PutAsync(url, formData);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ProductDTO>();
            }
            else
            {
                var response = await _httpClient.PutAsJsonAsync(url, productDTO);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ProductDTO>();
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/delete/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
