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
        public async Task<IActionResult> GetAllCategories([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            // 1. CHỐT CHẶN: Khởi tạo đường ống truy vấn (Chưa chạm vào Database vội)
            var query = _context.MedicineCategories.AsQueryable();

            // 2. BỘ LỌC TÌM KIẾM: Nếu Frontend có gửi chữ lên, lập tức ép màng lọc vào
            if (!string.IsNullOrWhiteSpace(search))
            {
                // Chuyển về chữ thường hết để tìm kiếm không phân biệt hoa/thường (tùy thuộc Collation của SQL)
                query = query.Where(c => c.CategoryName.Contains(search.Trim()));
            }

            // 3. ĐẾM THẬT: Đếm số lượng bản ghi SAU KHI đã đi qua màng lọc
            var totalItems = await query.CountAsync();

            // 4. LẤY DỮ LIỆU: Bắt đầu phân trang và kéo dữ liệu thật từ Database lên
            var categories = await query
                .OrderBy(c => c.CategoryId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new MedicineCategoryInputDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description
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
        /// [PUBLIC] Lấy thông tin chi tiết của một danh mục theo ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetCategoryById(int id)
        {
            var category = _context.MedicineCategories.FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound(new { message = $"Không tìm thấy danh mục với ID = {id}" });
            }

            var categoryDto = new MedicineCategoryInputDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };

            return Ok(categoryDto);
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