using Microsoft.EntityFrameworkCore;
using OrderingSystemData.Models;
using OrderingSystemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingSystemService
{
    public class CategoryService
    {
        private readonly OrderingSystemContext _context;

        public CategoryService(OrderingSystemContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var categoryDTOs = await _context.Categories
                .Select(c => new CategoryDTO
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,

                }).OrderBy(c => c.CategoryID)
                .ToListAsync();

            return categoryDTOs;
        }
        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var categoryDTO = await _context.Categories
                .Where(c => c.CategoryID == id)
                .Select(c => new CategoryDTO
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                })
                .FirstOrDefaultAsync();

            return categoryDTO;
        }

        public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDTO)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryDTO.CategoryName);
            if (existingCategory != null)
            {
                throw new InvalidOperationException($"Danh mục với tên '{categoryDTO.CategoryName}' đã tồn tại.");
            }


            var category = new Category
            {
                CategoryName = categoryDTO.CategoryName
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            categoryDTO.CategoryID = category.CategoryID;
            return categoryDTO;
        }


        public async Task<CategoryDTO> UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Danh mục với ID {id} không tồn tại");
            }
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryDTO.CategoryName && c.CategoryID != id);
            if (existingCategory != null)
            {
                throw new InvalidOperationException($"Danh mục với tên '{categoryDTO.CategoryName}' đã tồn tại");
            }



            category.CategoryName = categoryDTO.CategoryName;
;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    throw new KeyNotFoundException($"Danh mục với ID {id} không tồn tại");
                }
                else
                {
                    throw;
                }
            }

            return categoryDTO;
        }


        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false; 
            }

            var associatedProducts = await _context.Products
                .Where(p => p.CategoryID == id)
                .AnyAsync();

            if (associatedProducts)
            {
                return false;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }


        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
        }
    }
}
