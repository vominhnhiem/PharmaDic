package com.example.pharmadicmobile.dtos;

import com.google.gson.annotations.SerializedName;
import java.util.ArrayList;
import java.util.List;

public class MedicineDetailDto {
    @SerializedName(value = "MedicineId", alternate = {"medicineId"})
    private int medicineId;

    @SerializedName(value = "MedicineName", alternate = {"medicineName"})
    private String medicineName;

    @SerializedName(value = "CategoryName", alternate = {"categoryName"})
    private String categoryName;

    @SerializedName(value = "DosageForm", alternate = {"dosageForm"})
    private String dosageForm;

    @SerializedName(value = "Manufacturer", alternate = {"manufacturer"})
    private String manufacturer;

    @SerializedName(value = "Uses", alternate = {"uses"})
    private String uses;

    @SerializedName(value = "Dosage", alternate = {"dosage"})
    private String dosage;

    @SerializedName(value = "Contraindications", alternate = {"contraindications"})
    private String contraindications;

    @SerializedName(value = "SideEffects", alternate = {"sideEffects"})
    private String sideEffects;

    @SerializedName(value = "Note", alternate = {"note"})
    private String note;

    @SerializedName(value = "Ingredients", alternate = {"ingredients"})
    private List<IngredientDto> ingredients = new ArrayList<>();

    public int getMedicineId() { return medicineId; }
    public String getMedicineName() { return medicineName; }
    public String getCategoryName() { return categoryName; }
    public String getDosageForm() { return dosageForm; }
    public String getManufacturer() { return manufacturer; }
    public String getUses() { return uses; }
    public String getDosage() { return dosage; }
    public String getContraindications() { return contraindications; }
    public String getSideEffects() { return sideEffects; }
    public String getNote() { return note; }
    public List<IngredientDto> getIngredients() { return ingredients; }
}
