using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.Models;

namespace PharmaDicBackEnd.ApiService.Services;

public class MedicalChatbotService
{
    private readonly GroqAiService _groqService;
    private readonly DrugLookupAppContext _dbContext;

    public MedicalChatbotService(GroqAiService geminiService, DrugLookupAppContext dbContext)
    {
        _groqService = geminiService;
        _dbContext = dbContext;
    }

    public async Task<string> AskMedicalQuestionAsync(string question)
    {
        // 1. Dùng Gemini trích xuất từ khóa tìm kiếm (ngắn gọn)
        string extractPrompt = "Bạn là một trợ lý y tế. Hãy trích xuất 1 từ khóa quan trọng nhất (tên thuốc, tên bệnh, hoặc triệu chứng) từ câu hỏi sau để tìm kiếm trong cơ sở dữ liệu. Chỉ trả về đúng 1 từ khóa đó, không giải thích gì thêm.";
        string keyword = await _groqService.GenerateContentAsync(extractPrompt, question);
        keyword = keyword.Trim().Replace("\"", ""); // Xóa khoảng trắng hoặc ngoặc kép thừa

        // 2. Tìm kiếm trong SQL Database (tìm thuốc theo tên hoặc công dụng chứa từ khóa)
        var searchResults = await _dbContext.Medicines
            .Include(m => m.Category)
            .Where(m => m.MedicineName.Contains(keyword) || m.Uses.Contains(keyword))
            .Take(5) // Lấy tối đa 5 kết quả
            .ToListAsync();

        string context = "";
        if (searchResults.Any())
        {
            foreach (var med in searchResults)
            {
                var category = med.Category?.CategoryName ?? "Chưa phân loại";
                context += $"- Thuốc {med.MedicineName} (Nhóm: {category}): {med.Uses}\n";
            }
        }
        else
        {
            context = "Không tìm thấy thông tin cụ thể trong cơ sở dữ liệu cho câu hỏi này.";
        }

        // 3. Gửi thông tin tìm được cho Gemini để tạo câu trả lời cuối cùng
        string systemPrompt = @"Bạn là một chuyên gia y tế AI. 
Hãy trả lời câu hỏi của người dùng một cách chính xác, ngắn gọn và dễ hiểu.
Nếu có thông tin liên quan từ cơ sở dữ liệu (được cung cấp dưới đây), hãy ưu tiên sử dụng thông tin đó.
Lưu ý: Luôn có một cảnh báo nhỏ ở cuối câu trả lời rằng 'Thông tin chỉ mang tính tham khảo, vui lòng hỏi ý kiến bác sĩ'.";

        string userPrompt = $"Câu hỏi: {question}\n\nThông tin tham khảo từ CSDL:\n{context}";

        return await _groqService.GenerateContentAsync(systemPrompt, userPrompt);
    }
}
