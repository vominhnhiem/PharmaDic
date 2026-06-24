package com.example.pharmadicmobile.models;

import java.io.Serializable;

public class DrugInteraction implements Serializable {
    private int interactionId;
    private int medicineId1;
    private int medicineId2;
    private String severity;
    private String description;
    private String recommendation;
    private Medicine medicineId1Navigation;
    private Medicine medicineId2Navigation;

    public DrugInteraction() {}

    public int getInteractionId() { return interactionId; }
    public void setInteractionId(int interactionId) { this.interactionId = interactionId; }

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

    public Medicine getMedicineId1Navigation() { return medicineId1Navigation; }
    public void setMedicineId1Navigation(Medicine medicineId1Navigation) { this.medicineId1Navigation = medicineId1Navigation; }

    public Medicine getMedicineId2Navigation() { return medicineId2Navigation; }
    public void setMedicineId2Navigation(Medicine medicineId2Navigation) { this.medicineId2Navigation = medicineId2Navigation; }
}
