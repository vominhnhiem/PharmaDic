using BCrypt.Net; // Cần dùng thư viện này (hoặc thư viện băm mà bạn dùng trong AuthController)
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.DTOs;
using PharmaDicBackEnd.ApiService.Models;
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
        /// [ADMIN] Lấy danh sách toàn bộ người dùng
        /// </summary>
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users
                .Select(u => new UserResponseDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToList();

            return Ok(users);
        }

        /// <summary>
        /// [ADMIN] Cấp phát tài khoản mới cho nhân sự/người dùng
        /// </summary>
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Chống trùng lặp Email
            if (_context.Users.Any(u => u.Email == dto.Email))
            {
                return Conflict(new { message = "Email này đã tồn tại trong hệ thống." });
            }

            var newUser = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                // BẮT BUỘC BĂM MẬT KHẨU TRƯỚC KHI LƯU DB
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "Dược sĩ" : dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { message = "Tạo tài khoản thành công!", id = newUser.UserId });
        }

        /// <summary>
        /// [ADMIN] Cập nhật thông tin và quyền hạn của tài khoản
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return NotFound(new { message = $"Không tìm thấy tài khoản với ID = {id}" });

            // Kiểm tra xem email cập nhật có bị trùng với người khác không
            if (_context.Users.Any(u => u.Email == dto.Email && u.UserId != id))
            {
                return Conflict(new { message = "Email này đã được sử dụng bởi một tài khoản khác." });
            }

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Role = string.IsNullOrWhiteSpace(dto.Role) ? "Dược sĩ" : dto.Role;

            _context.SaveChanges();
            return Ok(new { message = "Cập nhật thông tin tài khoản thành công!" });
        }
        // Ví dụ thêm vào UserController.cs ở Backend
        [HttpGet("search-history/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetSearchHistory(int userId)
        {
            var history = await _context.SearchHistories
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.SearchDate)
                .ToListAsync();
            return Ok(history);
        }
        /// <summary>
        /// [ADMIN] Xóa tài khoản
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            // THUẬT TOÁN CHỐNG TỰ SÁT: Đọc ID của Admin đang gửi Request từ Token
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim != null && int.TryParse(currentUserIdClaim, out int currentUserId))
            {
                if (id == currentUserId)
                {
                    return BadRequest(new { message = "LỖI BẢO MẬT: Bạn không thể tự xóa chính tài khoản Admin đang đăng nhập của mình!" });
                }
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return NotFound(new { message = $"Không tìm thấy tài khoản với ID = {id}" });

            try
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return Ok(new { message = "Xóa tài khoản thành công!" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Không thể xóa tài khoản này vì họ đang có lịch sử tìm kiếm hoặc danh sách thuốc yêu thích. Cần xóa dữ liệu liên quan trước." });
            }
        }
    }
}