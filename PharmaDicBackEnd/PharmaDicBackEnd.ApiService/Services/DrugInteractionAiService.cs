using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.Models;

namespace PharmaDicBackEnd.ApiService.Services;

public class DrugInteractionAiService
{
    private readonly GroqAiService _groqService;
    private readonly DrugLookupAppContext _dbContext;

    public DrugInteractionAiService(GroqAiService geminiService, DrugLookupAppContext dbContext)
    {
        _groqService = geminiService;
        _dbContext = dbContext;
    }

    public async Task<string> CheckDrugInteractionsAsync(List<int> medicineIds)
    {
        if (medicineIds == null || medicineIds.Count < 2)
            return "Vui lòng cung cấp ít nhất 2 loại thuốc để kiểm tra tương tác.";

        var medicines = await _dbContext.Medicines
            .Where(m => medicineIds.Contains(m.MedicineId))
            .ToListAsync();

        if (medicines.Count < 2)
            return "Không tìm đủ thông tin thuốc trong cơ sở dữ liệu.";

        var interactions = await _dbContext.DrugInteractions
            .Where(di => medicineIds.Contains(di.MedicineId1) && medicineIds.Contains(di.MedicineId2))
            .ToListAsync();

        string dbContext = "Dữ liệu tương tác từ hệ thống:\n";
        foreach(var interaction in interactions)
        {
            var m1 = medicines.FirstOrDefault(m => m.MedicineId == interaction.MedicineId1)?.MedicineName;
            var m2 = medicines.FirstOrDefault(m => m.MedicineId == interaction.MedicineId2)?.MedicineName;
            dbContext += $"- Giữa {m1} và {m2}: Mức độ {interaction.Severity}. {interaction.Description}. Khuyến nghị: {interaction.Recommendation}\n";
        }

        string medList = string.Join(", ", medicines.Select(m => m.MedicineName));

        string systemPrompt = @"Bạn là một dược sĩ lâm sàng AI.
Người dùng muốn kiểm tra tương tác giữa các loại thuốc.
Hãy phân tích sự tương tác dựa trên cơ sở dữ liệu nội bộ được cung cấp. Nếu DB không có, hãy dùng kiến thức chuyên môn của bạn để phân tích.
Đưa ra cảnh báo mức độ nghiêm trọng và lời khuyên xử lý.";

        string userPrompt = $"Các loại thuốc cần kiểm tra: {medList}\n\n{dbContext}";

        return await _groqService.GenerateContentAsync(systemPrompt, userPrompt);
    }
}
