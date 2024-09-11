using OrderingSystemCustomerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace OrderingSystemCustomerService
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        private string BaseUrl = $"{Utils.BaseAddress}/Category";


        public CategoryService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CategoryDTO>>(BaseUrl);
            return response;
        }

        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            var response = await _httpClient.GetFromJsonAsync<CategoryDTO>(url);
            return response;
        }

        public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, categoryDTO);
            response.EnsureSuccessStatusCode(); // Throws if HTTP response status is not success
            return await response.Content.ReadFromJsonAsync<CategoryDTO>();
        }

        public async Task<CategoryDTO> UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            var url = $"{BaseUrl}/{id}";
            var response = await _httpClient.PutAsJsonAsync(url, categoryDTO);
            response.EnsureSuccessStatusCode(); // Throws if HTTP response status is not success
            return await response.Content.ReadFromJsonAsync<CategoryDTO>();
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var url = $"{BaseUrl}/{id}";
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
