package com.example.pharmadicmobile.dtos;

public class DrugInteractionInputDto {
    private int medicineId1;
    private int medicineId2;
    private String severity;
    private String description;
    private String recommendation;

    public DrugInteractionInputDto() {}

    public int getMedicineId1() { return medicineId1; }
    public void setMedicineId1(int medicineId1) { this.medicineId1 = medicineId1; }

    public int getMedicineId2() { return medicineId2; }
    public void setMedicineId2(int medicineId2) { this.medicineId2 = medicineId2; }

    public String getSeverity() { return severity; }
    public void setSeverity(String severity) { this.severity = severity; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public String getRecommendation() { return recommendation; }
    public void setRecommendation(String recommendation) { this.recommendation = recommendation; }
}
