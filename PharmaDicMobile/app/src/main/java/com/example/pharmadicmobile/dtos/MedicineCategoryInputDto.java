package com.example.pharmadicmobile.dtos;

public class MedicineCategoryInputDto {
    private String categoryName;
    private String description;

    public MedicineCategoryInputDto() {}

    public String getCategoryName() { return categoryName; }
    public void setCategoryName(String categoryName) { this.categoryName = categoryName; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }
}
