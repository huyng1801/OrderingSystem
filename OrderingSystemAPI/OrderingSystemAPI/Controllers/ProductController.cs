using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OrderingSystemDTO;
using OrderingSystemService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private const string ImageFolderPath = "Images";
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProducts();

                // Generate URLs for image files
                foreach (var product in products)
                {
                    if (!string.IsNullOrEmpty(product.Image))
                    {
                        product.Image = $"{Request.Scheme}://{Request.Host}/Images/{product.Image}";
                    }
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }



        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productService.GetProductsByCategory(categoryId);

                // Read image files and convert to binary data
                foreach (var product in products)
                {
                    if (!string.IsNullOrEmpty(product.Image))
                    {
                        product.Image = $"{Request.Scheme}://{Request.Host}/Images/{product.Image}";
                    }
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                {
                    return NotFound($"Sản phẩm với ID {id} không tìm thấy");
                }
                if (!string.IsNullOrEmpty(product.Image))
                {
                    product.Image = $"{Request.Scheme}://{Request.Host}/Images/{product.Image}";
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDTO productDTO, IFormFile imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePath = await SaveImage(imageFile);
                    productDTO.Image = imagePath;
                }

                var addedProduct = await _productService.AddProduct(productDTO);
                return CreatedAtAction(nameof(GetProductById), new { id = addedProduct.ProductID }, addedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDTO productDTO, IFormFile? imageFile = null)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePath = await SaveImage(imageFile);
                    productDTO.Image = imagePath;
                }

                var updatedProduct = await _productService.UpdateProduct(id, productDTO);
                return Ok(updatedProduct);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(ImageFolderPath, imageName);

            // Kiểm tra nếu thư mục không tồn tại, thì tạo thư mục mới
            if (!Directory.Exists(ImageFolderPath))
            {
                Directory.CreateDirectory(ImageFolderPath);
            }

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return imageName;
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProduct(id);
                if (!result)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
    }
}
