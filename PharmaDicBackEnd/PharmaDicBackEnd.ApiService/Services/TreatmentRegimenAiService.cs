using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.Models;

namespace PharmaDicBackEnd.ApiService.Services;

public class TreatmentRegimenAiService
{
    private readonly GroqAiService _groqService;
    private readonly DrugLookupAppContext _dbContext;

    public TreatmentRegimenAiService(GroqAiService geminiService, DrugLookupAppContext dbContext)
    {
        _groqService = geminiService;
        _dbContext = dbContext;
    }

    public async Task<string> SuggestTreatmentPlanAsync(string symptoms)
    {
        // 1. Dùng Gemini trích xuất từ khóa triệu chứng/bệnh lý chính
        string extractPrompt = "Bạn là một bác sĩ. Hãy trích xuất 1 hoặc 2 từ khóa ngắn gọn mô tả triệu chứng hoặc bệnh lý chính từ câu sau để tìm thuốc. Ví dụ: 'ho đờm', 'sốt', 'đau đầu'. Chỉ trả về từ khóa, không giải thích.";
        string keyword = await _groqService.GenerateContentAsync(extractPrompt, symptoms);
        keyword = keyword.Trim().Replace("\"", "");

        // 2. Tìm thuốc trong CSDL có công dụng chữa triệu chứng đó
        var searchResults = await _dbContext.Medicines
            .Include(m => m.Category)
            .Where(m => m.Uses.Contains(keyword) || m.MedicineName.Contains(keyword))
            .Take(5)
            .ToListAsync();

        string availableMedicines = "";
        if (searchResults.Any())
        {
            foreach (var med in searchResults)
            {
                var category = med.Category?.CategoryName ?? "Chưa phân loại";
                availableMedicines += $"- Thuốc: {med.MedicineName} (Nhóm: {category}) - Công dụng: {med.Uses}\n";
            }
        }
        else
        {
            availableMedicines = "Không tìm thấy thuốc đặc trị có sẵn trong CSDL cho triệu chứng này.";
        }

        // 3. Đưa thông tin cho Gemini gợi ý phác đồ
        string systemPrompt = @"Bạn là một bác sĩ AI.
Người dùng cung cấp các triệu chứng bệnh.
Nhiệm vụ của bạn là đưa ra một phác đồ điều trị gợi ý, BAO GỒM TÊN THUỐC, liều dùng tham khảo và lời khuyên lối sống.
Vui lòng ƯU TIÊN sử dụng các loại thuốc có sẵn trong nhà thuốc (được cung cấp trong mục Thuốc có sẵn). Nếu cần thiết, bạn có thể gợi ý thêm thuốc khác nhưng phải ghi rõ là 'Thuốc mua ngoài'.
Luôn cảnh báo người dùng phải đi khám bác sĩ để có chẩn đoán chính xác.
Lưu ý: Luôn có một cảnh báo nhỏ ở cuối câu trả lời rằng 'Thông tin chỉ mang tính tham khảo, vui lòng hỏi ý kiến bác sĩ'.";

        string userPrompt = $"Triệu chứng: {symptoms}\n\nThuốc có sẵn trong nhà thuốc phù hợp với triệu chứng:\n{availableMedicines}";

        return await _groqService.GenerateContentAsync(systemPrompt, userPrompt);
    }
}
