using System.ComponentModel.DataAnnotations;

namespace PharmaDicBackEnd.ApiService.DTOs
{
    public class CreateMedicineDto
    {
        [Required(ErrorMessage = "Tên thuốc không được để trống.")]
        [StringLength(150, ErrorMessage = "Tên thuốc không được vượt quá 150 ký tự.")]
        public string MedicineName { get; set; } = null!;

        public int? CategoryId { get; set; }
        public string? DosageForm { get; set; }
        public string? Strength { get; set; }
        public string? Manufacturer { get; set; }
        public string? Country { get; set; }
        public string? Uses { get; set; }
        public string? Dosage { get; set; }
        public string? Contraindications { get; set; }
        public string? SideEffects { get; set; }
        public string? Storage { get; set; }
        public string? Note { get; set; }
    }

    public class UpdateMedicineDto : CreateMedicineDto
    {
    }
}