using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // BẮT BUỘC THÊM ĐỂ DÙNG ASYNC/AWAIT
using PharmaDicBackEnd.ApiService.Models;
using PharmaDicBackEnd.ApiService.DTOs;
using System.Security.Claims;

namespace PharmaDicBackEnd.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Khóa bảo mật toàn cục: CHỈ ADMIN MỚI ĐƯỢC VÀO ĐÂY
    public class UserController : ControllerBase
    {
        private readonly DrugLookupAppContext _context;

        public UserController(DrugLookupAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// [ADMIN] Lấy danh sách người dùng (CÓ PHÂN TRANG & TÌM KIẾM)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            // 1. Khởi tạo đường ống truy vấn
            var query = _context.Users.AsQueryable();

            // 2. Màng lọc tìm kiếm: Tìm theo Email hoặc Họ Tên
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.Trim().ToLower();
                query = query.Where(u => u.Email.ToLower().Contains(searchLower) ||
                                         (u.FullName != null && u.FullName.ToLower().Contains(searchLower)));
            }

            // 3. Đếm tổng số lượng (sau khi lọc)
            var totalItems = await query.CountAsync();

            // 4. Phân trang và lấy dữ liệu thật
            var users = await query
                .OrderByDescending(u => u.CreatedAt) // Ưu tiên tài khoản mới tạo lên đầu
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserResponseDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            // 5. Đóng gói vào PagedResult (Phễu này đã khớp 100% với Frontend)
            var result = new PagedResult<UserResponseDto>
            {
                Items = users,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return Ok(result);
        }

        /// <summary>
        /// [ADMIN] Cấp phát tài khoản mới cho nhân sự/người dùng
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return Conflict(new { message = "Email này đã tồn tại trong hệ thống." });
            }

            var newUser = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "Dược sĩ" : dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(); // Chuyển sang Async

            return Ok(new { message = "Tạo tài khoản thành công!", id = newUser.UserId });
        }

        /// <summary>
        /// [ADMIN] Cập nhật thông tin và quyền hạn của tài khoản
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) return NotFound(new { message = $"Không tìm thấy tài khoản với ID = {id}" });

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.UserId != id))
            {
                return Conflict(new { message = "Email này đã được sử dụng bởi một tài khoản khác." });
            }

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Role = string.IsNullOrWhiteSpace(dto.Role) ? "Dược sĩ" : dto.Role;

            await _context.SaveChangesAsync(); // Chuyển sang Async
            return Ok(new { message = "Cập nhật thông tin tài khoản thành công!" });
        }

        /// <summary>
        /// [ADMIN] Xóa tài khoản
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim != null && int.TryParse(currentUserIdClaim, out int currentUserId))
            {
                if (id == currentUserId)
                {
                    return BadRequest(new { message = "LỖI BẢO MẬT: Bạn không thể tự xóa chính tài khoản Admin đang đăng nhập của mình!" });
                }
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) return NotFound(new { message = $"Không tìm thấy tài khoản với ID = {id}" });

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync(); // Chuyển sang Async
                return Ok(new { message = "Xóa tài khoản thành công!" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Không thể xóa tài khoản này vì họ đang có lịch sử tìm kiếm hoặc danh sách thuốc yêu thích. Cần xóa dữ liệu liên quan trước." });
            }
        }
    }
}