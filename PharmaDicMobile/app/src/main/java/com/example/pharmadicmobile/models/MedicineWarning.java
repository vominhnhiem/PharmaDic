package com.example.pharmadicmobile.models;

import java.io.Serializable;

public class MedicineWarning implements Serializable {
    private int warningId;
    private int medicineId;
    private String warningContent;
    private String warningLevel;
    private Medicine medicine;

    public MedicineWarning() {}

    public int getWarningId() { return warningId; }
    public void setWarningId(int warningId) { this.warningId = warningId; }

    public int getMedicineId() { return medicineId; }
    public void setMedicineId(int medicineId) { this.medicineId = medicineId; }

    public String getWarningContent() { return warningContent; }
    public void setWarningContent(String warningContent) { this.warningContent = warningContent; }

    public String getWarningLevel() { return warningLevel; }
    public void setWarningLevel(String warningLevel) { this.warningLevel = warningLevel; }

    public Medicine getMedicine() { return medicine; }
    public void setMedicine(Medicine medicine) { this.medicine = medicine; }
}
