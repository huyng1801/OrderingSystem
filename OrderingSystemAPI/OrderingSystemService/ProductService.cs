using Microsoft.EntityFrameworkCore;
using OrderingSystemData.Models;
using OrderingSystemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingSystemService
{
    public class ProductService
    {
        private readonly OrderingSystemContext _context;

        public ProductService(OrderingSystemContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDTO>> GetAllProducts()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return products.Select(p => new ProductDTO
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                Image = p.Image,
                CategoryID = p.CategoryID,
                CategoryName = p.Category?.CategoryName
            }).OrderByDescending(p => p.ProductID).ToList();
        }

        public async Task<ProductDTO> GetProductById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                return new ProductDTO
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Price = product.Price,
                    Image = product.Image,
                    CategoryID = product.CategoryID,
                    CategoryName = product.Category?.CategoryName
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ProductDTO>> GetProductsByCategory(int categoryId)
        {
            var products = await _context.Products
                                        .Where(p => p.CategoryID == categoryId)
                                        .ToListAsync();

            return products.Select(p => new ProductDTO
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                Image = p.Image,
                CategoryID = p.CategoryID,
                CategoryName = p.Category?.CategoryName
            }).OrderByDescending(p => p.ProductID).ToList();
        }

        public async Task<ProductDTO> AddProduct(ProductDTO productDTO)
        {
            var productExists = await _context.Categories.AnyAsync(c => c.CategoryID == productDTO.CategoryID);
            if (!productExists)
            {
                throw new InvalidOperationException("CategoryID does not exist");
            }

            var product = new Product
            {
                ProductName = productDTO.ProductName,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Image = productDTO.Image,
                CategoryID = productDTO.CategoryID
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            productDTO.ProductID = product.ProductID;
            return productDTO;
        }


        public async Task<ProductDTO> UpdateProduct(int productId, ProductDTO productDTO)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Sản phẩm với ID {productId} không tìm thấy");
            }

            // Kiểm tra xem CategoryID có tồn tại trong cơ sở dữ liệu không
            var productExists = await _context.Categories.AnyAsync(c => c.CategoryID == productDTO.CategoryID);
            if (!productExists)
            {
                throw new InvalidOperationException("Mã danh mục không tồn tại");
            }

            product.ProductName = productDTO.ProductName;
            product.Description = productDTO.Description;
            product.Price = productDTO.Price;
            product.CategoryID = productDTO.CategoryID;
            if(productDTO.Image != null)
            {
                product.Image = productDTO.Image;
            }
           
            product.CategoryID = productDTO.CategoryID;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(productId))
                {
                    throw new KeyNotFoundException($"Sản phẩm với ID {productId} không tìm thấy");
                }
                else
                {
                    throw;
                }
            }

            return productDTO;
        }


        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }

            bool hasOrders = await _context.OrderDetails.AnyAsync(od => od.ProductID == productId);

            if (hasOrders)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }


        private bool ProductExists(int productId)
        {
            return _context.Products.Any(p => p.ProductID == productId);
        }
    }
}
