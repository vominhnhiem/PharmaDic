package com.example.pharmadicmobile.models;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

public class MedicineCategory implements Serializable {
    private int categoryId;
    private String categoryName;
    private String description;
    private List<Medicine> medicines = new ArrayList<>();

    public MedicineCategory() {}

    public int getCategoryId() { return categoryId; }
    public void setCategoryId(int categoryId) { this.categoryId = categoryId; }

    public String getCategoryName() { return categoryName; }
    public void setCategoryName(String categoryName) { this.categoryName = categoryName; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public List<Medicine> getMedicines() { return medicines; }
    public void setMedicines(List<Medicine> medicines) { this.medicines = medicines; }
}
