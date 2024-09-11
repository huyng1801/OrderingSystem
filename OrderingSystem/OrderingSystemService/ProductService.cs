using Microsoft.AspNetCore.Http;
using OrderingSystemDTO;
using System.Net.Http.Json;

namespace OrderingSystemService
{
    public class ProductService : IProductService
    {
        private readonly string BaseUrl = $"{Utils.BaseAddress}/Product";

        public async Task<List<ProductDTO>> GetAllProducts()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<List<ProductDTO>>(BaseUrl);
                return response;
            }
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<ProductDTO>(url);
                return response;
            }
        }

        public async Task<List<ProductDTO>> GetProductsByCategory(int categoryId)
        {
            var url = $"{BaseUrl}/category/{categoryId}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<List<ProductDTO>>(url);
                return response;
            }
        }

        public async Task<ProductDTO> AddProduct(ProductDTO productDTO, IFormFile imageFile)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var formData = new MultipartFormDataContent();
                    formData.Add(new StringContent(productDTO.ProductName), "ProductName");
                    formData.Add(new StringContent(productDTO.Description == null ? "" : productDTO.Description), "Description");
                    formData.Add(new StringContent(productDTO.Price.ToString()), "Price");
                    formData.Add(new StringContent(productDTO.CategoryID.ToString()), "CategoryID");
                    if (imageFile != null)
                    {
                        formData.Add(new StreamContent(imageFile.OpenReadStream()), "imageFile", imageFile.FileName);
                    }

                    var response = await httpClient.PostAsync($"{BaseUrl}/add", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<ProductDTO>();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Rethrow the exception for the caller to handle
            }
        }


        public async Task<ProductDTO> UpdateProduct(int id, ProductDTO productDTO, IFormFile imageFile = null)
        {
            try
            {
                var url = $"{BaseUrl}/update/{id}";

                using (var httpClient = new HttpClient())
                {
                    var formData = new MultipartFormDataContent();
                    formData.Add(new StringContent(productDTO.ProductID.ToString()), "ProductID");
                    formData.Add(new StringContent(productDTO.ProductName), "ProductName");
                    formData.Add(new StringContent(productDTO.Description == null ? "" : productDTO.Description), "Description");
                    formData.Add(new StringContent(productDTO.Price.ToString()), "Price");
                    formData.Add(new StringContent(productDTO.CategoryID.ToString()), "CategoryID");
                    if (imageFile != null)
                    {
                        formData.Add(new StreamContent(imageFile.OpenReadStream()), "imageFile", imageFile.FileName);
                    }

                    var response = await httpClient.PutAsync(url, formData);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<ProductDTO>();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Rethrow the exception for the caller to handle
            }
        }


        public async Task<bool> DeleteProduct(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{BaseUrl}/delete/{id}");
                return response.IsSuccessStatusCode;
            }
        }
    }

    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProducts();
        Task<ProductDTO> GetProductById(int id);
        Task<List<ProductDTO>> GetProductsByCategory(int categoryId);
        Task<ProductDTO> AddProduct(ProductDTO productDTO, IFormFile imageFile);
        Task<ProductDTO> UpdateProduct(int id, ProductDTO productDTO, IFormFile imageFile = null);
        Task<bool> DeleteProduct(int id);
    }
}
