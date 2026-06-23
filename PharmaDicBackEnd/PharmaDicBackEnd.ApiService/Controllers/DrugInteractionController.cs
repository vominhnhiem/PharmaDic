using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaDicBackEnd.ApiService.Models;
using PharmaDicBackEnd.ApiService.DTOs;

namespace PharmaDicBackEnd.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Dược sĩ")]
    public class DrugInteractionController : ControllerBase
    {
        private readonly DrugLookupAppContext _context;

        public DrugInteractionController(DrugLookupAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Thêm cảnh báo tương tác giữa 2 loại thuốc
        /// </summary>
        [HttpPost]
        public IActionResult CreateInteraction([FromBody] DrugInteractionInputDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (dto.MedicineId1 == dto.MedicineId2)
            {
                return BadRequest(new { message = "Một loại thuốc không thể tự tương tác với chính nó." });
            }

            // KIỂM TRA LOGIC 2 CHIỀU: Chặn tạo trùng lặp dù đảo ngược ID
            var existingInteraction = _context.DrugInteractions.FirstOrDefault(i =>
                (i.MedicineId1 == dto.MedicineId1 && i.MedicineId2 == dto.MedicineId2) ||
                (i.MedicineId1 == dto.MedicineId2 && i.MedicineId2 == dto.MedicineId1)
            );

            if (existingInteraction != null)
            {
                return Conflict(new { message = "Cảnh báo tương tác giữa hai loại thuốc này đã tồn tại trong hệ thống." });
            }

            var interaction = new DrugInteraction
            {
                MedicineId1 = dto.MedicineId1,
                MedicineId2 = dto.MedicineId2,
                Severity = dto.Severity,
                Description = dto.Description,
                Recommendation = dto.Recommendation
            };

            _context.DrugInteractions.Add(interaction);
            _context.SaveChanges();

            return Ok(new { message = "Thêm cảnh báo tương tác thuốc thành công!", id = interaction.InteractionId });
        }

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Xóa cảnh báo tương tác
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteInteraction(int id)
        {
            var interaction = _context.DrugInteractions.FirstOrDefault(i => i.InteractionId == id);
            if (interaction == null) return NotFound(new { message = "Không tìm thấy dữ liệu tương tác để xóa." });

            _context.DrugInteractions.Remove(interaction);
            _context.SaveChanges();

            return Ok(new { message = "Xóa cảnh báo tương tác thành công!" });
        }
    }
}