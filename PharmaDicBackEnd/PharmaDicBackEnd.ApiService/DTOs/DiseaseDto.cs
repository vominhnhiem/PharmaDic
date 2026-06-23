namespace PharmaDicBackEnd.ApiService.DTOs
{
    public class DiseaseSummaryDto
    {
        public int DiseaseId { get; set; }
        public string DiseaseName { get; set; } = null!;
        public string? SymptomsList { get; set; }
    }

    public class DiseaseInputDto
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Tên bệnh lý không được để trống.")]
        [System.ComponentModel.DataAnnotations.StringLength(200, ErrorMessage = "Tên bệnh lý không được vượt quá 200 ký tự.")]
        public string DiseaseName { get; set; } = null!;

        public string? Description { get; set; }
        public string? WarningSigns { get; set; }
    }

    public class DiseaseDetailDto
    {
        public int DiseaseId { get; set; }
        public string DiseaseName { get; set; } = null!;
        public string? Description { get; set; }
        public string? WarningSigns { get; set; }

        public List<string> Symptoms { get; set; } = new List<string>();

        public List<SuggestedMedicineDto> SuggestedMedicines { get; set; } = new List<SuggestedMedicineDto>();
    }

    public class SuggestedMedicineDto
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = null!;
        public string? DosageForm { get; set; }
        public string? Note { get; set; }
    }
}