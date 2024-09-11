using Microsoft.AspNetCore.Mvc;
using OrderingSystemDTO;
using OrderingSystemService;

namespace OrderingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                var category = new CategoryDTO
                {
                    CategoryName = categoryDTO.CategoryName
                };
                var addedCategory = await _categoryService.AddCategoryAsync(category);
                return Ok(addedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.CategoryID)
            {
                return BadRequest();
            }

            var category = new CategoryDTO
            {
                CategoryID = id,
                CategoryName = categoryDTO.CategoryName
            };

            try
            {
                var updatedCategory = await _categoryService.UpdateCategory(id, category);
                return Ok(updatedCategory);
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


        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategory(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

    }
}
