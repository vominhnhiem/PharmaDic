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
            var response =
                await _chatbotService.AskMedicalQuestionAsync(request.Question);

            var history = new AIChatHistory
            {
                UserId = request.UserId,
                Question = request.Question,
                Answer = response,
                CreatedAt = DateTime.Now
            };

            _context.AIChatHistories.Add(history);
            await _context.SaveChangesAsync();

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
                Error = ex.Message,
                InnerException = ex.InnerException?.Message,
                InnerInnerException = ex.InnerException?.InnerException?.Message,
                StackTrace = ex.StackTrace
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

        return Ok(new
        {
            Message = "Đã xóa lịch sử chat."
        });
    }

    [HttpPost("check-interaction")]
    public async Task<IActionResult> CheckInteraction([FromBody] InteractionRequest request)
    {
        if (request.MedicineIds == null || request.MedicineIds.Count < 2)
            return BadRequest("Cần ít nhất 2 ID thuốc để kiểm tra tương tác.");

        try
        {
            var response =
                await _interactionService.CheckDrugInteractionsAsync(request.MedicineIds);

            return Ok(new { Answer = response });
        }
        catch (Exception ex)
        {
            return Ok(new
            {
                IsError = true,
                Error = ex.Message,
                InnerException = ex.InnerException?.Message,
                InnerInnerException = ex.InnerException?.InnerException?.Message,
                StackTrace = ex.StackTrace
            });
        }
    }

    [HttpPost("suggest-regimen")]
    public async Task<IActionResult> SuggestRegimen([FromBody] RegimenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Symptoms))
            return BadRequest("Triệu chứng không được để trống.");

        try
        {
            var response =
                await _regimenService.SuggestTreatmentPlanAsync(request.Symptoms);

            return Ok(new { Answer = response });
        }
        catch (Exception ex)
        {
            return Ok(new
            {
                IsError = true,
                Error = ex.Message,
                InnerException = ex.InnerException?.Message,
                InnerInnerException = ex.InnerException?.InnerException?.Message,
                StackTrace = ex.StackTrace
            });
        }
    }
   
}