using System.ComponentModel.DataAnnotations;

namespace PharmaDicBackEnd.ApiService.DTOs
{
    public class SymptomInputDto
    {
        [Required(ErrorMessage = "Tên triệu chứng không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên triệu chứng không được vượt quá 100 ký tự.")]
        public string SymptomName { get; set; } = null!;

        public string? Description { get; set; }
    }
}