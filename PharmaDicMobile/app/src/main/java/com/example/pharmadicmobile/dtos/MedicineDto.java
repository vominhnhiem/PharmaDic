package com.example.pharmadicmobile.dtos;

public class MedicineDto {
    private int medicineId;
    private String medicineName;
    private String categoryName;
    private String dosageForm;
    private String manufacturer;
    private String uses;

    public MedicineDto() {}

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
}
