using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaDicBackEnd.ApiService.Models;
using PharmaDicBackEnd.ApiService.DTOs;

namespace PharmaDicBackEnd.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineCategoryController : ControllerBase
    {
        private readonly DrugLookupAppContext _context;

        public MedicineCategoryController(DrugLookupAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous] 
        public IActionResult GetAllCategories()
        {
            var categories = _context.MedicineCategories
                .Select(c => new
                {
                    c.CategoryId,
                    c.CategoryName,
                    c.Description
                }).ToList();

            return Ok(categories);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCategory([FromBody] MedicineCategoryInputDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = new MedicineCategory
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description
            };

            _context.MedicineCategories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAllCategories), new { message = "Thêm danh mục thành công!", id = category.CategoryId });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCategory(int id, [FromBody] MedicineCategoryInputDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = _context.MedicineCategories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound(new { message = $"Không tìm thấy danh mục với ID = {id}" });
            }

            category.CategoryName = dto.CategoryName;
            category.Description = dto.Description;

            _context.SaveChanges();
            return Ok(new { message = "Cập nhật danh mục thành công!" });
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.MedicineCategories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound(new { message = $"Không tìm thấy danh mục với ID = {id} để xóa." });
            }

            try
            {
                _context.MedicineCategories.Remove(category);
                _context.SaveChanges();
                return Ok(new { message = "Xóa danh mục thành công!" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Không thể xóa danh mục này vì đang có các loại thuốc thuộc danh mục này. Hãy di dời hoặc xóa thuốc trước." });
            }
        }
    }
}