using System.ComponentModel.DataAnnotations;

namespace PharmaDicBackEnd.ApiService.DTOs
{
    public class DrugInteractionInputDto
    {
        [Required(ErrorMessage = "ID Thuốc thứ nhất không được để trống.")]
        public int MedicineId1 { get; set; }

        [Required(ErrorMessage = "ID Thuốc thứ hai không được để trống.")]
        public int MedicineId2 { get; set; }

        [StringLength(50, ErrorMessage = "Mức độ nghiêm trọng không vượt quá 50 ký tự.")]
        public string? Severity { get; set; }

        public string? Description { get; set; }

        public string? Recommendation { get; set; }
    }
}