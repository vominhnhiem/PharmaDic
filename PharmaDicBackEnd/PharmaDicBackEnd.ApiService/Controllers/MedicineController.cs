using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.Models;
using PharmaDicBackEnd.ApiService.DTOs;

namespace PharmaDicBackEnd.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly DrugLookupAppContext _context;

        public MedicineController(DrugLookupAppContext context)
        {
            _context = context;
        }
        /// <summary>
        /// [PUBLIC] Lấy danh sách tất cả các loại thuốc (Có phân trang)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMedicines([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // 1. Đảm bảo tham số hợp lệ
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            // 2. Đếm tổng số lượng thuốc trong DB
            var totalItems = await _context.Medicines.CountAsync();

            // 3. Truy vấn cắt dữ liệu theo trang
            var medicines = await _context.Medicines
                .Include(m => m.Category)
                .OrderBy(m => m.MedicineId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MedicineDto
                {
                    MedicineId = m.MedicineId,
                    MedicineName = m.MedicineName,
                    CategoryName = m.Category != null ? m.Category.CategoryName : "Chưa phân loại",
                    DosageForm = m.DosageForm,
                    Manufacturer = m.Manufacturer,
                    Uses = m.Uses
                })
                .ToListAsync();

            // 4. Đóng gói kết quả trả về
            var result = new PagedResult<MedicineDto>
            {
                Items = medicines,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return Ok(result);
        }
        /// <summary>
        /// [PUBLIC] Tra cứu danh sách thuốc theo từ khóa
        /// </summary>
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

        /// <summary>
        /// [PUBLIC] Lấy thông tin chi tiết của một loại thuốc theo ID
        /// </summary>
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

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Thêm mới một loại thuốc vào hệ thống
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Dược sĩ")]
        public IActionResult CreateMedicine([FromBody] CreateMedicineDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newMedicine = new Medicine
            {
                MedicineName = dto.MedicineName,
                CategoryId = dto.CategoryId,
                DosageForm = dto.DosageForm,
                Strength = dto.Strength,
                Manufacturer = dto.Manufacturer,
                Country = dto.Country,
                Uses = dto.Uses,
                Dosage = dto.Dosage,
                Contraindications = dto.Contraindications,
                SideEffects = dto.SideEffects,
                Storage = dto.Storage,
                Note = dto.Note
            };

            _context.Medicines.Add(newMedicine);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMedicineById), new { id = newMedicine.MedicineId }, new { message = "Thêm thuốc thành công!", id = newMedicine.MedicineId });
        }

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Cập nhật thông tin thuốc
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Dược sĩ")]
        public IActionResult UpdateMedicine(int id, [FromBody] UpdateMedicineDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicine = _context.Medicines.FirstOrDefault(m => m.MedicineId == id);

            if (medicine == null)
            {
                return NotFound(new { message = $"Không tìm thấy thuốc với ID = {id} để cập nhật." });
            }

            if (dto.CategoryId.HasValue)
            {
                var categoryExists = _context.MedicineCategories.Any(c => c.CategoryId == dto.CategoryId.Value);
                if (!categoryExists)
                {
                    return BadRequest(new { message = $"Danh mục thuốc với ID = {dto.CategoryId} không tồn tại trong hệ thống." });
                }
            }

            medicine.MedicineName = dto.MedicineName;
            medicine.CategoryId = dto.CategoryId;
            medicine.DosageForm = dto.DosageForm;
            medicine.Strength = dto.Strength;
            medicine.Manufacturer = dto.Manufacturer;
            medicine.Country = dto.Country;
            medicine.Uses = dto.Uses;
            medicine.Dosage = dto.Dosage;
            medicine.Contraindications = dto.Contraindications;
            medicine.SideEffects = dto.SideEffects;
            medicine.Storage = dto.Storage;
            medicine.Note = dto.Note;

            _context.SaveChanges();

            return Ok(new { message = "Cập nhật thông tin thuốc thành công!" });
        }

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Xóa vĩnh viễn một loại thuốc
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Dược sĩ")]
        public IActionResult DeleteMedicine(int id)
        {
            var medicine = _context.Medicines.FirstOrDefault(m => m.MedicineId == id);

            if (medicine == null)
            {
                return NotFound(new { message = $"Không tìm thấy thuốc với ID = {id} để xóa." });
            }

            try
            {
                _context.Medicines.Remove(medicine);
                _context.SaveChanges();
                return Ok(new { message = "Xóa thuốc khỏi hệ thống thành công!" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Không thể xóa thuốc này vì nó đang liên kết với dữ liệu hoạt chất hoặc bệnh lý khác. Hãy xóa liên kết trước." });
            }
        }

        /// <summary>
        /// [ADMIN/DƯỢC SĨ] Gắn danh sách Hoạt chất (kèm hàm lượng) vào một loại thuốc
        /// </summary>
        [HttpPost("{id}/ingredients")]
        [Authorize(Roles = "Admin,Dược sĩ")]
        public IActionResult AssignIngredientsToMedicine(int id, [FromBody] List<MedicineIngredientInputDto> ingredientsDto)
        {
            var medicine = _context.Medicines
                .Include(m => m.MedicineIngredients)
                .FirstOrDefault(m => m.MedicineId == id);

            if (medicine == null)
            {
                return NotFound(new { message = $"Không tìm thấy thuốc với ID = {id}." });
            }

            _context.MedicineIngredients.RemoveRange(medicine.MedicineIngredients);

            foreach (var item in ingredientsDto)
            {
                var ingredientExists = _context.Ingredients.Any(i => i.IngredientId == item.IngredientId);
                if (ingredientExists)
                {
                    _context.MedicineIngredients.Add(new MedicineIngredient
                    {
                        MedicineId = id,
                        IngredientId = item.IngredientId,
                        Amount = item.Amount
                    });
                }
            }

            _context.SaveChanges();
            return Ok(new { message = "Cập nhật danh sách hoạt chất và hàm lượng thành công!" });
        }
    }
}