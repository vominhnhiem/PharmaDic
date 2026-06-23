using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.Models;
using PharmaDicBackEnd.ApiService.DTOs;

namespace PharmaDicBackEnd.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Dược sĩ")]
    public class MedicineController : ControllerBase
    {
        private readonly DrugLookupAppContext _context;

        public MedicineController(DrugLookupAppContext context)
        {
            _context = context;
        }
        
        [HttpGet("search")]
        [AllowAnonymous]
        public IActionResult SearchMedicines([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Vui lòng nhập từ khóa tìm kiếm." });
            }

            var medicines = _context.Medicines
                .Include(m => m.Category)
                .Where(m => m.MedicineName.Contains(keyword))
                .Select(m => new MedicineDto
                {
                    MedicineId = m.MedicineId,
                    MedicineName = m.MedicineName,
                    CategoryName = m.Category != null ? m.Category.CategoryName : "Chưa phân loại",
                    DosageForm = m.DosageForm,
                    Manufacturer = m.Manufacturer,
                    Uses = m.Uses
                })
                .Take(50)
                .ToList();

            if (!medicines.Any())
            {
                return NotFound(new { message = "Không tìm thấy loại thuốc nào phù hợp." });
            }

            return Ok(medicines);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetMedicineById(int id)
        {
            var medicine = _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.MedicineIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                .Where(m => m.MedicineId == id)
                .Select(m => new MedicineDetailDto
                {
                    MedicineId = m.MedicineId,
                    MedicineName = m.MedicineName,
                    CategoryName = m.Category != null ? m.Category.CategoryName : "Chưa phân loại",
                    DosageForm = m.DosageForm,
                    Manufacturer = m.Manufacturer,
                    Uses = m.Uses,
                    Dosage = m.Dosage,
                    Contraindications = m.Contraindications,
                    SideEffects = m.SideEffects,
                    Ingredients = m.MedicineIngredients.Select(mi => new IngredientDto
                    {
                        IngredientName = mi.Ingredient.IngredientName,
                        Amount = mi.Amount
                    }).ToList()
                })
                .FirstOrDefault();

            if (medicine == null)
            {
                return NotFound(new { message = $"Không tìm thấy thuốc với ID = {id}." });
            }

            return Ok(medicine);
        }
    }
}