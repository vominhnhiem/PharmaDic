package com.example.pharmadicmobile.dtos;

public class DashboardOverviewDto {
    private int totalMedicines;
    private int totalDiseases;
    private int totalUsers;
    private int totalCategories;

    public DashboardOverviewDto() {}

    public int getTotalMedicines() { return totalMedicines; }
    public void setTotalMedicines(int totalMedicines) { this.totalMedicines = totalMedicines; }

    public int getTotalDiseases() { return totalDiseases; }
    public void setTotalDiseases(int totalDiseases) { this.totalDiseases = totalDiseases; }

    public int getTotalUsers() { return totalUsers; }
    public void setTotalUsers(int totalUsers) { this.totalUsers = totalUsers; }

    public int getTotalCategories() { return totalCategories; }
    public void setTotalCategories(int totalCategories) { this.totalCategories = totalCategories; }
}
