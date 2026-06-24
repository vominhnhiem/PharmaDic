using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PharmaDicBackEnd.ApiService.Models;
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
        private readonly DrugLookupAppContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(DrugLookupAppContext context, IConfiguration configuration)
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

            // SỬA DÒNG NÀY: Dùng BCrypt.Verify để so sánh mật khẩu thô với mã băm trong DB
            bool isPasswordValid = false;

            try
            {
                // Kiểm tra xem mật khẩu dưới DB có đúng định dạng BCrypt không (luôn bắt đầu bằng "$2")
                if (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash.StartsWith("$2"))
                {
                    // Nếu đúng chuẩn bảo mật, dùng code của Mobile Dev để xác thực
                    isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
                }
                else
                {
                    // NẾU LÀ TÀI KHOẢN CŨ: So sánh chữ thô bình thường để Web không bị sập
                    isPasswordValid = (request.Password == user.PasswordHash);
                }
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Bẫy lỗi an toàn: Lỡ dính chuỗi rác, hệ thống chỉ báo sai Pass chứ không được phép Crash
                isPasswordValid = false;
            }

            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không chính xác." });
            }

            // 2. Nếu đúng mật khẩu, tiến hành tạo JWT Token
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "Đăng nhập thành công",
                token = token,
                role = user.Role
            });
        }

        // Hàm nội bộ dùng để phát hành Token
        private string GenerateJwtToken(Models.User user)
        {
            // Lấy các setting từ appsettings.json
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            if (!string.IsNullOrEmpty(user.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Dược sĩ"));
            }

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