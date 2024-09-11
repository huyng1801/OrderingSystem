using OrderingSystemDTO;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OrderingSystemService
{
    public class TableService : ITableService
    {
        private readonly string BaseUrl = $"{Utils.BaseAddress}/Table";

        public async Task<List<TableDTO>> GetAllTables()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<List<TableDTO>>(BaseUrl);
                return response;
            }
        }

        public async Task<TableDTO> GetTableById(string id)
        {
            var url = $"{BaseUrl}/{id}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<TableDTO>(url);
                return response;
            }
        }

        public async Task<TableDTO> AddTable(TableDTO tableDTO)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{BaseUrl}/add";
                var response = await httpClient.PostAsJsonAsync(url, tableDTO);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<TableDTO>();
            }
        }

        public async Task<bool> UpdateTable(string id, TableDTO tableDTO)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{BaseUrl}/{id}/update";
                var response = await httpClient.PutAsJsonAsync(url, tableDTO);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeleteTable(string id)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{BaseUrl}/delete/{id}";
                var response = await httpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
        }
    }

    public interface ITableService
    {
        Task<List<TableDTO>> GetAllTables();
        Task<TableDTO> GetTableById(string id);
        Task<TableDTO> AddTable(TableDTO tableDTO);
        Task<bool> UpdateTable(string id, TableDTO tableDTO);
        Task<bool> DeleteTable(string id);
    }
}
