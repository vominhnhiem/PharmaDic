package com.example.pharmadicmobile.dtos;

import com.google.gson.annotations.SerializedName;

public class MedicineDto {
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

    @SerializedName(value = "ImageUrl", alternate = {"imageUrl"})
    private String imageUrl;

    public int getMedicineId() { return medicineId; }
    public String getMedicineName() { return medicineName; }
    public String getCategoryName() { return categoryName; }
    public String getDosageForm() { return dosageForm; }
    public String getManufacturer() { return manufacturer; }
    public String getUses() { return uses; }
    public String getDosage() { return dosage; }
    public String getContraindications() { return contraindications; }
    public String getImageUrl() { return imageUrl; }
}
