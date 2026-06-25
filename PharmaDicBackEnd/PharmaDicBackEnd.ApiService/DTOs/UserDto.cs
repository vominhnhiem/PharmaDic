    using System.ComponentModel.DataAnnotations;

    namespace PharmaDicBackEnd.ApiService.DTOs
    {
        public class UserResponseDto
        {
            public int UserId { get; set; }
            public string FullName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string? Role { get; set; }
            public DateTime? CreatedAt { get; set; }
        }

        public class UserCreateDto
        {
            [Required(ErrorMessage = "Họ tên không được để trống.")]
            public string FullName { get; set; } = null!;

            [Required(ErrorMessage = "Email không được để trống.")]
            [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
            public string Email { get; set; } = null!;

            [Required(ErrorMessage = "Mật khẩu không được để trống.")]
            [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên.")]
            public string Password { get; set; } = null!;

            public string? Role { get; set; }
        }

        public class UserUpdateDto
        {
            [Required(ErrorMessage = "Họ tên không được để trống.")]
            public string FullName { get; set; } = null!;

            [Required(ErrorMessage = "Email không được để trống.")]
            [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
            public string Email { get; set; } = null!;

            public string? Role { get; set; }
        }
    }