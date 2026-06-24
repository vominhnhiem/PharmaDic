using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.DTOs;
using PharmaDicBackEnd.ApiService.Models;

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

        /// <summary>
        /// [PUBLIC] Lấy danh sách danh mục thuốc (Có phân trang)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var totalItems = await _context.MedicineCategories.CountAsync();

            var categories = await _context.MedicineCategories
                .OrderBy(c => c.CategoryId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new MedicineCategoryInputDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description // Tùy chỉnh các thuộc tính này cho khớp với DTO của bạn
                })
                .ToListAsync();

            var result = new PagedResult<MedicineCategoryInputDto>
            {
                Items = categories,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return Ok(result);
        }

        /// <summary>
        /// [ADMIN] Thêm mới một danh mục thuốc vào hệ thống
        /// </summary>
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

        /// <summary>
        /// [ADMIN] Cập nhật thông tin của một danh mục thuốc
        /// </summary>
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

        /// <summary>
        /// [ADMIN] Xóa vĩnh viễn một danh mục thuốc (Có kiểm tra ràng buộc)
        /// </summary>
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