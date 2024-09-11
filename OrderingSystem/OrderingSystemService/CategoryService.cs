using OrderingSystemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace OrderingSystemService
{
    public class CategoryService : ICategoryService
    {
        private string BaseUrl = $"{Utils.BaseAddress}/Category";
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<List<CategoryDTO>>(BaseUrl);
                return response;
            }
        }

        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var url = $"{BaseUrl}/{id}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<CategoryDTO>(url);
                return response;
            }
        }

        public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDTO)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(BaseUrl, categoryDTO);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CategoryDTO>();
                }
                else
                {
                    return null;
                }
            }
        }


        public async Task<CategoryDTO> UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            var url = $"{BaseUrl}/{id}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsJsonAsync(url, categoryDTO);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CategoryDTO>();
                }
                else
                {
                    return null;
                }
            }
        }


        public async Task<bool> DeleteCategory(int id)
        {
            var url = $"{BaseUrl}/{id}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
        }
    }
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO> GetCategoryById(int id);
        Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDTO);
        Task<CategoryDTO> UpdateCategory(int id, CategoryDTO categoryDTO);
        Task<bool> DeleteCategory(int id);
    }
}
