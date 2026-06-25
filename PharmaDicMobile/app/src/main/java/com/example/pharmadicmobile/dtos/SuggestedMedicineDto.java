package com.example.pharmadicmobile.dtos;

public class SuggestedMedicineDto {
    private int medicineId;
    private String medicineName;
    private String dosageForm;
    private String note;

    public SuggestedMedicineDto() {}

    public int getMedicineId() { return medicineId; }
    public void setMedicineId(int medicineId) { this.medicineId = medicineId; }

    public String getMedicineName() { return medicineName; }
    public void setMedicineName(String medicineName) { this.medicineName = medicineName; }

    public String getDosageForm() { return dosageForm; }
    public void setDosageForm(String dosageForm) { this.dosageForm = dosageForm; }

    public String getNote() { return note; }
    public void setNote(String note) { this.note = note; }
}
