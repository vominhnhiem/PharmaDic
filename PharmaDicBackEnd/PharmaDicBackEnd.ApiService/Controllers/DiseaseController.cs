using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.Models;
using PharmaDicBackEnd.ApiService.DTOs;
using PharmaDicBackEnd.ApiService.Models;

namespace PharmaDicBackEnd.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiseaseController : ControllerBase
    {
        private readonly DrugLookupAppContext _context; // Đã đổi tên context theo đúng file tự sinh của bạn

        public DiseaseController(DrugLookupAppContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public IActionResult SearchDiseases([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Vui lòng nhập từ khóa tìm kiếm." });
            }

            var diseasesFromDb = _context.Diseases
                .Include(d => d.Symptoms)
                .Where(d => d.DiseaseName.Contains(keyword) ||
                            d.Symptoms.Any(s => s.SymptomName.Contains(keyword)))
                .Take(50)
                .ToList(); // Kéo data về bộ nhớ vật lý của máy chủ

            if (!diseasesFromDb.Any())
            {
                return NotFound(new { message = "Không tìm thấy bệnh lý nào phù hợp." });
            }

            var result = diseasesFromDb.Select(d => new DiseaseSummaryDto
            {
                DiseaseId = d.DiseaseId,
                DiseaseName = d.DiseaseName,
                SymptomsList = string.Join(", ", d.Symptoms.Select(s => s.SymptomName))
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetDiseaseById(int id)
        {
            var disease = _context.Diseases
                .Include(d => d.Symptoms)
                .Include(d => d.DiseaseMedicines)
                    .ThenInclude(dm => dm.Medicine)
                .Where(d => d.DiseaseId == id)
                .Select(d => new DiseaseDetailDto
                {
                    DiseaseId = d.DiseaseId,
                    DiseaseName = d.DiseaseName,
                    Description = d.Description,
                    WarningSigns = d.WarningSigns,

                    Symptoms = d.Symptoms.Select(s => s.SymptomName).ToList(),

                    SuggestedMedicines = d.DiseaseMedicines.Select(dm => new SuggestedMedicineDto
                    {
                        MedicineId = dm.Medicine.MedicineId,
                        MedicineName = dm.Medicine.MedicineName,
                        DosageForm = dm.Medicine.DosageForm,
                        Note = dm.Note
                    }).ToList()
                })
                .FirstOrDefault();

            if (disease == null)
            {
                return NotFound(new { message = $"Không tìm thấy bệnh lý với ID = {id}." });
            }

            return Ok(disease);
        }
    }
}