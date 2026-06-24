using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.DTOs
{
    /// <summary>
    /// Số liệu tổng số lượng các thực thể trong hệ thống
    /// </summary>
    public class DashboardOverviewDto
    {
        public int TotalMedicines { get; set; }
        public int TotalDiseases { get; set; }
        public int TotalUsers { get; set; }
        public int TotalCategories { get; set; }

        public List<int> WeeklyTrends { get; set; } = new List<int>();
    }

    /// <summary>
    /// Cấu trúc phục vụ cho Biểu đồ tròn phân bố thuốc theo danh mục
    /// </summary>
    public class CategoryDistributionDto
    {
        public string CategoryName { get; set; } = null!;
        public int MedicineCount { get; set; }
    }

    /// <summary>
    /// Cấu trúc phục vụ cho Biểu đồ đường/cột xu hướng đăng ký người dùng
    /// </summary>
    public class UserRegistrationTrendDto
    {
        public string TimeLabel { get; set; } = null!; // Ví dụ: "Tháng 05/2026", "Tháng 06/2026"
        public int UserCount { get; set; }
    }
}