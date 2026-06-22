namespace PharmaDicBackEnd.ApiService.DTOs
{
    public class MedicineDetailDto
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = null!;
        public string? CategoryName { get; set; }
        public string? DosageForm { get; set; }
        public string? Manufacturer { get; set; }
        public string? Uses { get; set; }
        public string? Dosage { get; set; }
        public string? Contraindications { get; set; }
        public string? SideEffects { get; set; }

        
        public List<IngredientDto> Ingredients { get; set; } = new List<IngredientDto>();
    }

    public class IngredientDto
    {
        public string IngredientName { get; set; } = null!;
        public string? Amount { get; set; } // Hàm lượng (VD: 500mg)
    }
}