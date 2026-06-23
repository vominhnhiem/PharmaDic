using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaDicBackEnd.ApiService.Models;
using PharmaDicBackEnd.ApiService.DTOs;

namespace PharmaDicBackEnd.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Dược sĩ")] // Khóa bảo mật toàn cục
    public class SymptomController : ControllerBase
    {
        private readonly DrugLookupAppContext _context;

        public SymptomController(DrugLookupAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// [PUBLIC] Lấy toàn bộ danh sách triệu chứng (Dùng cho cả Mobile và Web)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllSymptoms()
        {
            var symptoms = _context.Symptoms
                .Select(s => new { s.SymptomId, s.SymptomName, s.Description })
                .ToList();
            return Ok(symptoms);
        }

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Thêm mới triệu chứng vào từ điển
        /// </summary>
        [HttpPost]
        public IActionResult CreateSymptom([FromBody] SymptomInputDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var symptom = new Symptom
            {
                SymptomName = dto.SymptomName,
                Description = dto.Description
            };

            _context.Symptoms.Add(symptom);
            _context.SaveChanges();

            return Ok(new { message = "Thêm triệu chứng thành công!", id = symptom.SymptomId });
        }

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Cập nhật thông tin triệu chứng
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateSymptom(int id, [FromBody] SymptomInputDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var symptom = _context.Symptoms.FirstOrDefault(s => s.SymptomId == id);
            if (symptom == null) return NotFound(new { message = $"Không tìm thấy triệu chứng ID = {id}" });

            symptom.SymptomName = dto.SymptomName;
            symptom.Description = dto.Description;

            _context.SaveChanges();
            return Ok(new { message = "Cập nhật triệu chứng thành công!" });
        }

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Xóa triệu chứng khỏi từ điển
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteSymptom(int id)
        {
            var symptom = _context.Symptoms.FirstOrDefault(s => s.SymptomId == id);
            if (symptom == null) return NotFound(new { message = $"Không tìm thấy triệu chứng ID = {id}" });

            try
            {
                _context.Symptoms.Remove(symptom);
                _context.SaveChanges();
                return Ok(new { message = "Xóa triệu chứng thành công!" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Không thể xóa triệu chứng này vì nó đang được gắn vào một số bệnh lý." });
            }
        }
    }
}