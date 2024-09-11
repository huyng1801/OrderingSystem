using Microsoft.EntityFrameworkCore;
using OrderingSystemData.Models;
using OrderingSystemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingSystemAPI.Services
{
    public class TableService
    {
        private readonly OrderingSystemContext _context;

        public TableService(OrderingSystemContext context)
        {
            _context = context;
        }

        public async Task<List<TableDTO>> GetAllTables()
        {
            var tables = await _context.Tables.ToListAsync();
            return tables.Select(ConvertToDTO).ToList();
        }

        public async Task<TableDTO> GetTableById(string tableId)
        {
            var table = await _context.Tables.FindAsync(tableId);
            return ConvertToDTO(table);
        }

        public async Task<TableDTO> AddTable(TableDTO tableDTO)
        {
            var table = ConvertToEntity(tableDTO);
            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
            return ConvertToDTO(table);
        }
        public async Task<bool> UpdateTableOccupiedStatus(string tableId, TableDTO tableDTO)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null || tableDTO == null)
            {
                return false;
            }

            table.IsOccupied = tableDTO.IsOccupied;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTable(string tableId)
        {
            bool isTableInUse = await _context.Orders.AnyAsync(o => o.TableID == tableId);
            if (isTableInUse)
            {
                return false;
            }

            var table = await _context.Tables.FindAsync(tableId);
            if (table == null)
            {
                return false;
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return true;
        }

        private TableDTO ConvertToDTO(Table table)
        {
            return new TableDTO
            {
                TableID = table.TableID,
                IsOccupied = table.IsOccupied
            };
        }

        private Table ConvertToEntity(TableDTO tableDTO)
        {
            return new Table
            {
                TableID = tableDTO.TableID,
                IsOccupied = tableDTO.IsOccupied
        };
        }

    }
}
