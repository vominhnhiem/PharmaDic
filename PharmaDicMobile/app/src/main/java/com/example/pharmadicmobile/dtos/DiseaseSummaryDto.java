package com.example.pharmadicmobile.dtos;

public class DiseaseSummaryDto {
    private int diseaseId;
    private String diseaseName;
    private String symptomsList;

    public DiseaseSummaryDto() {}

    public int getDiseaseId() { return diseaseId; }
    public void setDiseaseId(int diseaseId) { this.diseaseId = diseaseId; }

    public String getDiseaseName() { return diseaseName; }
    public void setDiseaseName(String diseaseName) { this.diseaseName = diseaseName; }

    public String getSymptomsList() { return symptomsList; }
    public void setSymptomsList(String symptomsList) { this.symptomsList = symptomsList; }
}
