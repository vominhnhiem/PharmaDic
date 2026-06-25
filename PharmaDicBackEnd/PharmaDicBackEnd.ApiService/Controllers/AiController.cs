using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.DTOs;
using PharmaDicBackEnd.ApiService.Models;
using PharmaDicBackEnd.ApiService.Services;

namespace PharmaDicBackEnd.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiController : ControllerBase
{
    private readonly MedicalChatbotService _chatbotService;
    private readonly DrugInteractionAiService _interactionService;
    private readonly TreatmentRegimenAiService _regimenService;
    private readonly DrugLookupAppContext _context;

    public AiController(
        MedicalChatbotService chatbotService,
        DrugInteractionAiService interactionService,
        TreatmentRegimenAiService regimenService,
        DrugLookupAppContext context)
    {
        _chatbotService = chatbotService;
        _interactionService = interactionService;
        _regimenService = regimenService;
        _context = context;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return BadRequest("Câu hỏi không được để trống.");

        try
        {
            // 1. Gọi AI lấy câu trả lời
            var response = await _chatbotService.AskMedicalQuestionAsync(request.Question);

            // 2. Kiểm tra UserId có tồn tại trong DB không để tránh lỗi Foreign Key
            var userExists = await _context.Users.AnyAsync(u => u.UserId == request.UserId);

            if (!userExists)
            {
                // Nếu User không tồn tại, vẫn trả về câu trả lời nhưng báo lỗi không lưu được lịch sử
                return Ok(new
                {
                    Answer = response,
                    IsError = true,
                    Error = "Không thể lưu lịch sử: UserId " + request.UserId + " không tồn tại trong hệ thống."
                });
            }

            // 3. Tạo đối tượng lịch sử
            var history = new AIChatHistory
            {
                UserId = request.UserId,
                Question = request.Question,
                Answer = response,
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.AIChatHistories.Add(history);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Lỗi này thường do nội dung Question hoặc Answer quá dài vượt quá giới hạn DB (NVARCHAR(MAX) vs NVARCHAR(255))
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return Ok(new
                {
                    Answer = response,
                    IsError = true,
                    Error = "Lỗi Database khi lưu lịch sử: " + innerMessage
                });
            }

            return Ok(new
            {
                Answer = response
            });
        }
        catch (Exception ex)
        {
            return Ok(new
            {
                IsError = true,
                Error = "Lỗi hệ thống: " + ex.Message,
                Detail = ex.InnerException?.Message
            });
        }
    }

    [HttpGet("history/{userId}")]
    public async Task<IActionResult> GetHistory(int userId)
    {
        var histories = await _context.AIChatHistories
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(histories);
    }

    [HttpDelete("history/{chatId}")]
    public async Task<IActionResult> DeleteHistory(int chatId)
    {
        var history = await _context.AIChatHistories
            .FirstOrDefaultAsync(x => x.ChatId == chatId);

        if (history == null)
            return NotFound();

        _context.AIChatHistories.Remove(history);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Đã xóa lịch sử chat." });
    }

    [HttpPost("check-interaction")]
    public async Task<IActionResult> CheckInteraction([FromBody] InteractionRequest request)
    {
        if (request.MedicineIds == null || request.MedicineIds.Count < 2)
            return BadRequest("Cần ít nhất 2 ID thuốc để kiểm tra tương tác.");

        try
        {
            var response = await _interactionService.CheckDrugInteractionsAsync(request.MedicineIds);
            return Ok(new { Answer = response });
        }
        catch (Exception ex)
        {
            return Ok(new { IsError = true, Error = ex.Message });
        }
    }

    [HttpPost("suggest-regimen")]
    public async Task<IActionResult> SuggestRegimen([FromBody] RegimenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Symptoms))
            return BadRequest("Triệu chứng không được để trống.");

        try
        {
            var response = await _regimenService.SuggestTreatmentPlanAsync(request.Symptoms);
            return Ok(new { Answer = response });
        }
        catch (Exception ex)
        {
            return Ok(new { IsError = true, Error = ex.Message });
        }
    }
}