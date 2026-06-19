using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PharmaDicBackEnd.ApiService.Data;
using PharmaDicBackEnd.ApiService.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PharmaDicBackEnd.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        // Tiêm (Inject) DbContext để gọi Database và IConfiguration để đọc appsettings.json
        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 1. Tìm user trong CSDL dựa trên Email
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không chính xác." });
            }

            // [LỖ HỔNG BẢO MẬT]: Ở đây đang so sánh chuỗi mật khẩu trần (PlainText).
            // Dữ liệu từ file SQL của bạn chưa có cơ chế rắc muối (Salting).
            // Sau này bạn BẮT BUỘC phải dùng thư viện BCrypt.Net để Verify Hash chỗ này.
            // Tạm thời để test API, chúng ta so sánh trực tiếp.
            if (user.PasswordHash != request.Password)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không chính xác." });
            }

            // 2. Nếu đúng mật khẩu, tiến hành tạo JWT Token
            var token = GenerateJwtToken(user);

            // Trả về Token cho team Mobile
            return Ok(new
            {
                message = "Đăng nhập thành công",
                token = token,
                role = user.Role // Gửi kèm Role để Mobile dễ phân quyền giao diện
            });
        }

        // Hàm nội bộ dùng để phát hành Token
        private string GenerateJwtToken(Models.User user)
        {
            // Lấy các setting từ appsettings.json
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            // Nhét dữ liệu của Dược sĩ vào bên trong thân Token (Claims)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role ?? "Người dùng") // Rất quan trọng cho TASK-114
            };

            // Cấu hình thuật toán mã hóa
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(double.Parse(jwtSettings["ExpireDays"]!)),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
