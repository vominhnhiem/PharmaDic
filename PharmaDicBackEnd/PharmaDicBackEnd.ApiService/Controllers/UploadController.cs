using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaDicBackEnd.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Dược sĩ")] // Chỉ cho phép quản trị viên tải ảnh lên hệ thống
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// [ADMIN] Upload hình ảnh (thuốc, bệnh lý) lên server
        /// </summary>
        [HttpPost("image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            // 1. Kiểm tra file rỗng
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Vui lòng chọn một file ảnh hợp lệ." });
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { message = "Chỉ chấp nhận định dạng ảnh hợp lệ (.jpg, .png, .jpeg, .webp)." });
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest(new { message = "Dung lượng ảnh không được vượt quá 5MB." });
            }

            var newFileName = $"{Guid.NewGuid()}{extension}";
            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var exactFilePath = Path.Combine(uploadFolder, newFileName);

            using (var stream = new FileStream(exactFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"/uploads/{newFileName}";

            return Ok(new
            {
                message = "Upload ảnh thành công!",
                imageUrl = fileUrl
            });
        }
    }
}