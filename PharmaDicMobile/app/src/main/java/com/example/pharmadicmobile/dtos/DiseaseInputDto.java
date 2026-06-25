package com.example.pharmadicmobile.dtos;

public class DiseaseInputDto {
    private String diseaseName;
    private String description;
    private String warningSigns;

    public DiseaseInputDto() {}

    public String getDiseaseName() { return diseaseName; }
    public void setDiseaseName(String diseaseName) { this.diseaseName = diseaseName; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public String getWarningSigns() { return warningSigns; }
    public void setWarningSigns(String warningSigns) { this.warningSigns = warningSigns; }
}
