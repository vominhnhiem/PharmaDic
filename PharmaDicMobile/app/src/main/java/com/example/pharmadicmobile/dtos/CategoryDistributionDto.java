package com.example.pharmadicmobile.dtos;

public class CategoryDistributionDto {
    private String categoryName;
    private int medicineCount;

    public CategoryDistributionDto() {}

    public String getCategoryName() { return categoryName; }
    public void setCategoryName(String categoryName) { this.categoryName = categoryName; }

    public int getMedicineCount() { return medicineCount; }
    public void setMedicineCount(int medicineCount) { this.medicineCount = medicineCount; }
}
