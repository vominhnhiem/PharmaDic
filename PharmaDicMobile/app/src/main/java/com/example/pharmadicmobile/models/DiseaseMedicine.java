package com.example.pharmadicmobile.models;

import java.io.Serializable;

public class DiseaseMedicine implements Serializable {
    private int diseaseId;
    private int medicineId;
    private String note;
    private Disease disease;
    private Medicine medicine;

    public DiseaseMedicine() {}

    public int getDiseaseId() { return diseaseId; }
    public void setDiseaseId(int diseaseId) { this.diseaseId = diseaseId; }

    public int getMedicineId() { return medicineId; }
    public void setMedicineId(int medicineId) { this.medicineId = medicineId; }

    public String getNote() { return note; }
    public void setNote(String note) { this.note = note; }

    public Disease getDisease() { return disease; }
    public void setDisease(Disease disease) { this.disease = disease; }

    public Medicine getMedicine() { return medicine; }
    public void setMedicine(Medicine medicine) { this.medicine = medicine; }
}
