using OrderingSystemDTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OrderingSystemService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string BaseUrl = $"{Utils.BaseAddress}/Employee";

        public async Task<EmployeeDTO> Login(LoginDTO loginDTO)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{BaseUrl}/login";
                var response = await httpClient.PostAsJsonAsync(url, loginDTO);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<EmployeeDTO>();
            }
        }

        public async Task<EmployeeDTO> GetEmployeeById(string id)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{BaseUrl}/{id}";
                var response = await httpClient.GetFromJsonAsync<EmployeeDTO>(url);
                return response;
            }
        }

        public async Task<EmployeeDTO> AddEmployee(EmployeeDTO employeeDTO, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{BaseUrl}/add";

                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(employeeDTO.EmployeeID), "EmployeeID");
                formData.Add(new StringContent(employeeDTO.EmployeeName), "EmployeeName");
                formData.Add(new StringContent(employeeDTO.PhoneNumber), "PhoneNumber");
                formData.Add(new StringContent(employeeDTO.Address), "Address");
                formData.Add(new StringContent(employeeDTO.Role.ToString()), "Role");
                formData.Add(new StringContent(password), "Password");

                var response = await httpClient.PostAsync(url, formData);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<EmployeeDTO>();
            }
        }



        public async Task<EmployeeDTO> UpdateEmployee(EmployeeDTO employeeDTO, string password = null)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{BaseUrl}/update";

                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(employeeDTO.EmployeeID), "EmployeeID");
                formData.Add(new StringContent(employeeDTO.EmployeeName), "EmployeeName");
                formData.Add(new StringContent(employeeDTO.PhoneNumber), "PhoneNumber");
                formData.Add(new StringContent(employeeDTO.Address), "Address");

                if (!string.IsNullOrEmpty(password))
                {
                    formData.Add(new StringContent(password), "Password");
                }

                var response = await httpClient.PutAsync(url, formData);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<EmployeeDTO>();
            }
        }


        public async Task<List<EmployeeDTO>> GetAllEmployees()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetFromJsonAsync<List<EmployeeDTO>>(BaseUrl);
                return response;
            }
        }

        public async Task<bool> DeleteEmployee(string employeeID)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{BaseUrl}/{employeeID}";
                var response = await httpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
        }
    }

    public interface IEmployeeService
    {
        Task<EmployeeDTO> Login(LoginDTO loginDTO);
        Task<EmployeeDTO> GetEmployeeById(string id);
        Task<EmployeeDTO> AddEmployee(EmployeeDTO employeeDTO, string password);
        Task<EmployeeDTO> UpdateEmployee(EmployeeDTO employeeDTO, string password = null);
        Task<List<EmployeeDTO>> GetAllEmployees();
        Task<bool> DeleteEmployee(string employeeID);
    }
}
