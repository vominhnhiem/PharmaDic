package com.example.pharmadicmobile.models;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class Medicine implements Serializable {
    private int medicineId;
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
    private Date createdAt;
    private MedicineCategory category;
    private List<DiseaseMedicine> diseaseMedicines = new ArrayList<>();
    private List<DrugInteraction> drugInteractionMedicineId1Navigations = new ArrayList<>();
    private List<DrugInteraction> drugInteractionMedicineId2Navigations = new ArrayList<>();
    private List<Favorite> favorites = new ArrayList<>();
    private List<MedicineIngredient> medicineIngredients = new ArrayList<>();
    private List<MedicineWarning> medicineWarnings = new ArrayList<>();

    public Medicine() {}

    public int getMedicineId() { return medicineId; }
    public void setMedicineId(int medicineId) { this.medicineId = medicineId; }

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

    public Date getCreatedAt() { return createdAt; }
    public void setCreatedAt(Date createdAt) { this.createdAt = createdAt; }

    public MedicineCategory getCategory() { return category; }
    public void setCategory(MedicineCategory category) { this.category = category; }

    public List<DiseaseMedicine> getDiseaseMedicines() { return diseaseMedicines; }
    public void setDiseaseMedicines(List<DiseaseMedicine> diseaseMedicines) { this.diseaseMedicines = diseaseMedicines; }

    public List<DrugInteraction> getDrugInteractionMedicineId1Navigations() { return drugInteractionMedicineId1Navigations; }
    public void setDrugInteractionMedicineId1Navigations(List<DrugInteraction> drugInteractionMedicineId1Navigations) { this.drugInteractionMedicineId1Navigations = drugInteractionMedicineId1Navigations; }

    public List<DrugInteraction> getDrugInteractionMedicineId2Navigations() { return drugInteractionMedicineId2Navigations; }
    public void setDrugInteractionMedicineId2Navigations(List<DrugInteraction> drugInteractionMedicineId2Navigations) { this.drugInteractionMedicineId2Navigations = drugInteractionMedicineId2Navigations; }

    public List<Favorite> getFavorites() { return favorites; }
    public void setFavorites(List<Favorite> favorites) { this.favorites = favorites; }

    public List<MedicineIngredient> getMedicineIngredients() { return medicineIngredients; }
    public void setMedicineIngredients(List<MedicineIngredient> medicineIngredients) { this.medicineIngredients = medicineIngredients; }

    public List<MedicineWarning> getMedicineWarnings() { return medicineWarnings; }
    public void setMedicineWarnings(List<MedicineWarning> medicineWarnings) { this.medicineWarnings = medicineWarnings; }
}
