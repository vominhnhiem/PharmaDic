package com.example.pharmadicmobile.dtos;

public class CreateMedicineDto {
    private String medicineName;
    private Integer categoryId;
    private String dosageForm;
    private String strength;
    private String manufacturer;
    private String country;
    private String uses;
    private String dosage;
    private String contraindications;
    private String sideEffects;
    private String storage;
    private String note;

    public CreateMedicineDto() {}

    public String getMedicineName() { return medicineName; }
    public void setMedicineName(String medicineName) { this.medicineName = medicineName; }

    public Integer getCategoryId() { return categoryId; }
    public void setCategoryId(Integer categoryId) { this.categoryId = categoryId; }

    public String getDosageForm() { return dosageForm; }
    public void setDosageForm(String dosageForm) { this.dosageForm = dosageForm; }

    public String getStrength() { return strength; }
    public void setStrength(String strength) { this.strength = strength; }

    public String getManufacturer() { return manufacturer; }
    public void setManufacturer(String manufacturer) { this.manufacturer = manufacturer; }

    public String getCountry() { return country; }
    public void setCountry(String country) { this.country = country; }

    public String getUses() { return uses; }
    public void setUses(String uses) { this.uses = uses; }

    public String getDosage() { return dosage; }
    public void setDosage(String dosage) { this.dosage = dosage; }

    public String getContraindications() { return contraindications; }
    public void setContraindications(String contraindications) { this.contraindications = contraindications; }

    public String getSideEffects() { return sideEffects; }
    public void setSideEffects(String sideEffects) { this.sideEffects = sideEffects; }

    public String getStorage() { return storage; }
    public void setStorage(String storage) { this.storage = storage; }

    public String getNote() { return note; }
    public void setNote(String note) { this.note = note; }
}
