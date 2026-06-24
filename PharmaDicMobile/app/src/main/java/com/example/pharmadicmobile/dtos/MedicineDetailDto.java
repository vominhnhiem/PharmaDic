package com.example.pharmadicmobile.dtos;

import java.util.ArrayList;
import java.util.List;

public class MedicineDetailDto {
    private int medicineId;
    private String medicineName;
    private String categoryName;
    private String dosageForm;
    private String manufacturer;
    private String uses;
    private String dosage;
    private String contraindications;
    private String sideEffects;
    private List<IngredientDto> ingredients = new ArrayList<>();

    public MedicineDetailDto() {}

    public int getMedicineId() { return medicineId; }
    public void setMedicineId(int medicineId) { this.medicineId = medicineId; }

    public String getMedicineName() { return medicineName; }
    public void setMedicineName(String medicineName) { this.medicineName = medicineName; }

    public String getCategoryName() { return categoryName; }
    public void setCategoryName(String categoryName) { this.categoryName = categoryName; }

    public String getDosageForm() { return dosageForm; }
    public void setDosageForm(String dosageForm) { this.dosageForm = dosageForm; }

    public String getManufacturer() { return manufacturer; }
    public void setManufacturer(String manufacturer) { this.manufacturer = manufacturer; }

    public String getUses() { return uses; }
    public void setUses(String uses) { this.uses = uses; }

    public String getDosage() { return dosage; }
    public void setDosage(String dosage) { this.dosage = dosage; }

    public String getContraindications() { return contraindications; }
    public void setContraindications(String contraindications) { this.contraindications = contraindications; }

    public String getSideEffects() { return sideEffects; }
    public void setSideEffects(String sideEffects) { this.sideEffects = sideEffects; }

    public List<IngredientDto> getIngredients() { return ingredients; }
    public void setIngredients(List<IngredientDto> ingredients) { this.ingredients = ingredients; }
}
