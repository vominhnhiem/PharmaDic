using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.Models;

namespace PharmaDicBackEnd.ApiService.Services;

public class MedicalChatbotService
{
    private readonly GroqAiService _groqService;
    private readonly DrugLookupAppContext _dbContext;

    public MedicalChatbotService(
        GroqAiService groqService,
        DrugLookupAppContext dbContext)
    {
        _groqService = groqService;
        _dbContext = dbContext;
    }

    public async Task<string> AskMedicalQuestionAsync(
        int userId,
        string question)
    {
        // 1. Trích xuất từ khóa
        string extractPrompt = @"
Bạn là một trợ lý y tế.
Hãy trích xuất duy nhất 1 từ khóa quan trọng nhất
(tên thuốc, bệnh hoặc triệu chứng) từ câu hỏi dưới đây.
Chỉ trả về đúng 1 từ khóa, không giải thích.";

        string keyword = await _groqService.GenerateContentAsync(
            extractPrompt,
            question);

        keyword = keyword.Trim().Replace("\"", "");

        // Nếu AI không trích xuất được từ khóa
        if (string.IsNullOrWhiteSpace(keyword))
        {
            keyword = question;
        }

        // 2. Tìm kiếm trong CSDL
        var searchResults = await _dbContext.Medicines
            .Include(m => m.Category)
            .Where(m =>
                (m.MedicineName ?? "").Contains(keyword) ||
                (m.Uses ?? "").Contains(keyword))
            .OrderBy(m => m.MedicineName)
            .Take(5)
            .ToListAsync();

        // 3. Tạo context cho AI
        string context;

        if (searchResults.Any())
        {
            context = "Thông tin từ cơ sở dữ liệu:\n";

            foreach (var med in searchResults)
            {
                var category = med.Category?.CategoryName
                               ?? "Chưa phân loại";

                context +=
                    $"- Thuốc: {med.MedicineName}\n" +
                    $"  Nhóm: {category}\n" +
                    $"  Công dụng: {med.Uses}\n" +
                    $"  Tác dụng phụ: {med.SideEffects}\n\n";
            }
        }
        else
        {
            context = "Không tìm thấy thông tin trong cơ sở dữ liệu.";
        }

        // 4. Prompt trả lời cuối cùng
        string systemPrompt = @"
Bạn là một chuyên gia dược lâm sàng AI.

Nhiệm vụ:
- Trả lời chính xác, ngắn gọn, dễ hiểu.
- Ưu tiên thông tin lấy từ cơ sở dữ liệu.
- Nếu CSDL không có, sử dụng kiến thức y khoa của bạn.
- Không chẩn đoán chắc chắn bệnh.
- Luôn kết thúc bằng:

'Thông tin chỉ mang tính tham khảo, vui lòng hỏi ý kiến bác sĩ.'";

        string userPrompt =
            $"Câu hỏi: {question}\n\n" +
            $"{context}";

        string answer = await _groqService.GenerateContentAsync(
            systemPrompt,
            userPrompt);

        // 5. Lưu lịch sử chat
        var history = new AIChatHistory
        {
            UserId = userId,
            Question = question,
            Answer = answer,
            CreatedAt = DateTime.Now
        };

        _dbContext.AIChatHistories.Add(history);
        await _dbContext.SaveChangesAsync();

        return answer;
    }
}