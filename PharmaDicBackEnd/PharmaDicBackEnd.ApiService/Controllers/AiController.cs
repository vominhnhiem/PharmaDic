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
    private readonly DrugLookupAppContext _dbContext;

    public AiController(
        MedicalChatbotService chatbotService,
        DrugInteractionAiService interactionService,
        TreatmentRegimenAiService regimenService,
        DrugLookupAppContext dbContext)
    {
        _chatbotService = chatbotService;
        _interactionService = interactionService;
        _regimenService = regimenService;
        _dbContext = dbContext;
    }

    // Chat AI
    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return BadRequest("Câu hỏi không được để trống.");

        try
        {
            var response = await _chatbotService
                .AskMedicalQuestionAsync(
                    request.UserId,
                    request.Question);

            return Ok(new
            {
                Answer = response
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                IsError = true,
                Error = ex.Message
            });
        }
    }

    // Kiểm tra tương tác thuốc
    [HttpPost("check-interaction")]
    public async Task<IActionResult> CheckInteraction([FromBody] InteractionRequest request)
    {
        if (request.MedicineIds == null || request.MedicineIds.Count < 2)
            return BadRequest("Cần ít nhất 2 ID thuốc để kiểm tra tương tác.");

        try
        {
            var response = await _interactionService
                .CheckDrugInteractionsAsync(request.MedicineIds);

            return Ok(new
            {
                Answer = response
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                IsError = true,
                Error = ex.Message
            });
        }
    }

    // Gợi ý phác đồ điều trị
    [HttpPost("suggest-regimen")]
    public async Task<IActionResult> SuggestRegimen([FromBody] RegimenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Symptoms))
            return BadRequest("Triệu chứng không được để trống.");

        try
        {
            var response = await _regimenService
                .SuggestTreatmentPlanAsync(request.Symptoms);

            return Ok(new
            {
                Answer = response
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                IsError = true,
                Error = ex.Message
            });
        }
    }

    // Xem lịch sử chat AI theo User
    [HttpGet("history/{userId}")]
    public async Task<IActionResult> GetHistory(int userId)
    {
        var histories = await _dbContext.AIChatHistories
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new
            {
                x.ChatId,
                x.Question,
                x.Answer,
                x.CreatedAt
            })
            .ToListAsync();

        return Ok(histories);
    }

    // Xóa một lịch sử chat
    [HttpDelete("history/{chatId}")]
    public async Task<IActionResult> DeleteHistory(int chatId)
    {
        var history = await _dbContext.AIChatHistories
            .FindAsync(chatId);

        if (history == null)
            return NotFound("Không tìm thấy lịch sử.");

        _dbContext.AIChatHistories.Remove(history);
        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            Message = "Đã xóa lịch sử thành công."
        });
    }
}