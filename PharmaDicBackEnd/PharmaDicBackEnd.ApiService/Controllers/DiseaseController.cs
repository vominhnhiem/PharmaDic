using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.Models;
using PharmaDicBackEnd.ApiService.DTOs;

namespace PharmaDicBackEnd.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Dược sĩ")] // Khóa bảo mật toàn cục cho Controller
    public class DiseaseController : ControllerBase
    {
        private readonly DrugLookupAppContext _context;

        public DiseaseController(DrugLookupAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// [PUBLIC] Tra cứu danh sách bệnh lý theo từ khóa
        /// </summary>
        [HttpGet("search")]
        [AllowAnonymous] // Ngoại lệ: Mở cho toàn bộ người dùng
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
                .ToList();

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

        /// <summary>
        /// [PUBLIC] Xem chi tiết thông tin một bệnh lý bao gồm triệu chứng và thuốc gợi ý
        /// </summary>
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

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Thêm mới một bệnh lý vào hệ thống
        /// </summary>
        [HttpPost]
        public IActionResult CreateDisease([FromBody] DiseaseInputDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newDisease = new Disease
            {
                DiseaseName = dto.DiseaseName,
                Description = dto.Description,
                WarningSigns = dto.WarningSigns
            };

            _context.Diseases.Add(newDisease);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetDiseaseById), new { id = newDisease.DiseaseId }, new { message = "Thêm bệnh lý thành công!", id = newDisease.DiseaseId });
        }

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Cập nhật thông tin bệnh lý
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateDisease(int id, [FromBody] DiseaseInputDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var disease = _context.Diseases.FirstOrDefault(d => d.DiseaseId == id);
            if (disease == null)
            {
                return NotFound(new { message = $"Không tìm thấy bệnh lý với ID = {id}" });
            }

            disease.DiseaseName = dto.DiseaseName;
            disease.Description = dto.Description;
            disease.WarningSigns = dto.WarningSigns;

            _context.SaveChanges();
            return Ok(new { message = "Cập nhật thông tin bệnh lý thành công!" });
        }

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Xóa vĩnh viễn bệnh lý
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteDisease(int id)
        {
            var disease = _context.Diseases.FirstOrDefault(d => d.DiseaseId == id);
            if (disease == null)
            {
                return NotFound(new { message = $"Không tìm thấy bệnh lý với ID = {id} để xóa." });
            }

            try
            {
                _context.Diseases.Remove(disease);
                _context.SaveChanges();
                return Ok(new { message = "Xóa bệnh lý thành công!" });
            }
            catch (Exception)
            {
                // Bẻ gãy lỗi 500 khi vướng khóa ngoại ở bảng Symptoms hoặc DiseaseMedicines
                return BadRequest(new { message = "Không thể xóa bệnh lý này vì đang có các triệu chứng hoặc thuốc gợi ý liên kết. Hãy xóa các liên kết trước." });
            }
        }
        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Gắn danh sách triệu chứng vào một bệnh lý cụ thể
        /// </summary>
        [HttpPost("{id}/symptoms")]
        public IActionResult AssignSymptomsToDisease(int id, [FromBody] List<int> symptomIds)
        {
            var disease = _context.Diseases
                .Include(d => d.Symptoms)
                .FirstOrDefault(d => d.DiseaseId == id);

            if (disease == null)
            {
                return NotFound(new { message = $"Không tìm thấy bệnh lý với ID = {id}." });
            }

            var validSymptoms = _context.Symptoms
                .Where(s => symptomIds.Contains(s.SymptomId))
                .ToList();

            if (validSymptoms.Count != symptomIds.Count)
            {
                return BadRequest(new { message = "Một hoặc nhiều ID triệu chứng gửi lên không tồn tại trong hệ thống." });
            }

            disease.Symptoms.Clear();
            foreach (var symptom in validSymptoms)
            {
                disease.Symptoms.Add(symptom);
            }

            _context.SaveChanges();

            return Ok(new { message = "Cập nhật danh sách triệu chứng cho bệnh lý thành công!" });
        }
    }
}