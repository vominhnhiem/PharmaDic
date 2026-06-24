using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDicBackEnd.ApiService.Models;
using PharmaDicBackEnd.ApiService.DTOs;
using System;
using System.Linq;

namespace PharmaDicBackEnd.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Bảo mật tuyệt đối: Chỉ Admin tối cao mới được xem số liệu kinh doanh
    public class DashboardController : ControllerBase
    {
        private readonly DrugLookupAppContext _context;

        public DashboardController(DrugLookupAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// [ADMIN] Lấy số liệu tổng quan hệ thống
        /// </summary>
        [HttpGet("overview")]
        public IActionResult GetOverviewMetrics()
        {
            var overview = new DashboardOverviewDto
            {
                TotalMedicines = _context.Medicines.Count(),
                TotalDiseases = _context.Diseases.Count(),
                TotalUsers = _context.Users.Count(),
                TotalCategories = _context.MedicineCategories.Count()
            };

            return Ok(overview);
        }

        /// <summary>
        /// [ADMIN] Lấy dữ liệu phân bố số lượng thuốc theo từng danh mục
        /// </summary>
        [HttpGet("category-distribution")]
        public IActionResult GetCategoryDistribution()
        {
            var distribution = _context.Medicines
                .Include(m => m.Category)
                .GroupBy(m => m.Category != null ? m.Category.CategoryName : "Chưa phân loại")
                .Select(g => new CategoryDistributionDto
                {
                    CategoryName = g.Key,
                    MedicineCount = g.Count()
                })
                .ToList();

            return Ok(distribution);
        }

        /// <summary>
        /// [ADMIN] Lấy xu hướng đăng ký tài khoản mới của người dùng theo từng ngày trong 30 ngày gần đây
        /// </summary>
        [HttpGet("user-trends")]
        public IActionResult GetUserRegistrationTrends()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var rawUsersData = _context.Users
                .Where(u => u.CreatedAt >= thirtyDaysAgo)
                .Select(u => u.CreatedAt)
                .ToList();

            var trends = rawUsersData
                .Where(date => date.HasValue)
                .GroupBy(date => date.Value.Date)
                .OrderBy(g => g.Key)
                .Select(g => new UserRegistrationTrendDto
                {
                    TimeLabel = g.Key.ToString("dd/MM/yyyy"),
                    UserCount = g.Count()
                })
                .ToList();

            return Ok(trends);
        }
    }
}