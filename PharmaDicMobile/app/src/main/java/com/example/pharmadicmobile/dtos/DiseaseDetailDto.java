package com.example.pharmadicmobile.dtos;

import java.util.ArrayList;
import java.util.List;

public class DiseaseDetailDto {
    private int diseaseId;
    private String diseaseName;
    private String description;
    private String warningSigns;
    private List<String> symptoms = new ArrayList<>();
    private List<SuggestedMedicineDto> suggestedMedicines = new ArrayList<>();

    public DiseaseDetailDto() {}

    public int getDiseaseId() { return diseaseId; }
    public void setDiseaseId(int diseaseId) { this.diseaseId = diseaseId; }

    public String getDiseaseName() { return diseaseName; }
    public void setDiseaseName(String diseaseName) { this.diseaseName = diseaseName; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public String getWarningSigns() { return warningSigns; }
    public void setWarningSigns(String warningSigns) { this.warningSigns = warningSigns; }

    public List<String> getSymptoms() { return symptoms; }
    public void setSymptoms(List<String> symptoms) { this.symptoms = symptoms; }

    public List<SuggestedMedicineDto> getSuggestedMedicines() { return suggestedMedicines; }
    public void setSuggestedMedicines(List<SuggestedMedicineDto> suggestedMedicines) { this.suggestedMedicines = suggestedMedicines; }
}
