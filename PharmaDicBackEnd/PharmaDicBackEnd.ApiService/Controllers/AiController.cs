using Microsoft.AspNetCore.Mvc;
using PharmaDicBackEnd.ApiService.DTOs;
using PharmaDicBackEnd.ApiService.Services;

namespace PharmaDicBackEnd.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiController : ControllerBase
{
    private readonly MedicalChatbotService _chatbotService;
    private readonly DrugInteractionAiService _interactionService;
    private readonly TreatmentRegimenAiService _regimenService;

    public AiController(
        MedicalChatbotService chatbotService, 
        DrugInteractionAiService interactionService, 
        TreatmentRegimenAiService regimenService)
    {
        _chatbotService = chatbotService;
        _interactionService = interactionService;
        _regimenService = regimenService;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return BadRequest("Câu hỏi không được để trống.");

        try
        {
            var response = await _chatbotService.AskMedicalQuestionAsync(request.Question);
            return Ok(new { Answer = response });
        }
        catch (Exception ex)
        {
            return Ok(new { IsError = true, Error = ex.Message, StackTrace = ex.StackTrace });
        }
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
            return Ok(new { IsError = true, Error = ex.Message, StackTrace = ex.StackTrace });
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
            return Ok(new { IsError = true, Error = ex.Message, StackTrace = ex.StackTrace });
        }
    }
}
