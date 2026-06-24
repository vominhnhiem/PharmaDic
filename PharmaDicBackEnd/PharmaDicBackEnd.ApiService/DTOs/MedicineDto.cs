namespace PharmaDicBackEnd.ApiService.DTOs
{
    public class MedicineDto
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = null!;
        public string? CategoryName { get; set; }
        public string? DosageForm { get; set; }
        public string? Manufacturer { get; set; }
        public string? Uses { get; set; }

    }
}