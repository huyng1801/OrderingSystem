using Microsoft.EntityFrameworkCore;
using OrderingSystemData.Models;
using OrderingSystemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderingSystemService
{
    public class EmployeeService
    {
        private readonly OrderingSystemContext _context;

        public EmployeeService(OrderingSystemContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return employees.Select(ConvertToDTO).ToList();
        }

        public async Task<EmployeeDTO> GetEmployeeById(string employeeID)
        {
            var employee = await _context.Employees.FindAsync(employeeID);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Nhân viên với ID {employeeID} không tìm thấy");
            }

            return ConvertToDTO(employee);
        }

        public async Task<EmployeeDTO> AddEmployee(EmployeeDTO employeeDTO, string password)
        {
            // Check if the employee ID already exists
            bool isEmployeeExists = await _context.Employees.AnyAsync(e => e.EmployeeID == employeeDTO.EmployeeID);
            if (isEmployeeExists)
            {
                throw new InvalidOperationException($"Mã nhân viên '{employeeDTO.EmployeeID}' đã tồn tại trong hệ thống.");
            }

     

    

            // Convert DTO to entity
            var employee = ConvertToEntity(employeeDTO, password);

            // Add employee to the context and save changes
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Convert entity back to DTO and return
            return ConvertToDTO(employee);
        }

        public async Task<EmployeeDTO> UpdateEmployee(EmployeeDTO employeeDTO, string password = null)
        {
            var existingEmployee = await _context.Employees.FindAsync(employeeDTO.EmployeeID);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Nhân với với ID {employeeDTO.EmployeeID} không tìm thấy");
            }

            existingEmployee.EmployeeName = employeeDTO.EmployeeName;
            existingEmployee.PhoneNumber = employeeDTO.PhoneNumber;
            existingEmployee.Address = employeeDTO.Address;
            existingEmployee.Role = employeeDTO.Role;

            if (!string.IsNullOrWhiteSpace(password))
            {
                existingEmployee.Password = CreatePasswordHash(password);
            }

            await _context.SaveChangesAsync();

            return ConvertToDTO(existingEmployee);
        }

        public async Task<bool> DeleteEmployee(string employeeID)
        {
            var employee = await _context.Employees.FindAsync(employeeID);
            if (employee == null)
            {
                return false;
            }

            bool hasOrders = await _context.Orders.AnyAsync(o => o.EmployeeID == employeeID);
            if (hasOrders)
            {
                return false;
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<EmployeeDTO> Login(string employeeID, string password)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == employeeID);
            if (employee == null || !VerifyPasswordHash(password, employee.Password))
            {
                return null;
            }

            return ConvertToDTO(employee);
        }

        private string CreatePasswordHash(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            string hashedPassword = CreatePasswordHash(password);
            return storedHash.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
        }

        private EmployeeDTO ConvertToDTO(Employee employee)
        {
            return new EmployeeDTO
            {
                EmployeeID = employee.EmployeeID,
                EmployeeName = employee.EmployeeName,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                Role = employee.Role
            };
        }

        private Employee ConvertToEntity(EmployeeDTO employeeDTO, string password)
        {
            return new Employee
            {
                EmployeeID = employeeDTO.EmployeeID,
                EmployeeName = employeeDTO.EmployeeName,
                PhoneNumber = employeeDTO.PhoneNumber,
                Address = employeeDTO.Address,
                Role = employeeDTO.Role,
                Password = CreatePasswordHash(password)
            };
        }
    }
}
