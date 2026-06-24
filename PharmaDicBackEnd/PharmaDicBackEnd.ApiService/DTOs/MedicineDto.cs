namespace PharmaDicBackEnd.ApiService.DTOs
{
    public class MedicineDto
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string CategoryName { get; set; }
        public string DosageForm { get; set; }
        public string Manufacturer { get; set; }
        public string Uses { get; set; }

        // THÊM 2 DÒNG NÀY
        public string Dosage { get; set; }
        public string Contraindications { get; set; }
    }
}