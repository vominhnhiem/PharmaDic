using System.ComponentModel.DataAnnotations;

namespace PharmaDicBackEnd.ApiService.DTOs
{
    public class MedicineCategoryInputDto
    {
        public int CategoryId { get; set; } // Số nguyên thì không được dùng StringLength

        [Required(ErrorMessage = "Tên danh mục thuốc không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự.")]
        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }
    }
}