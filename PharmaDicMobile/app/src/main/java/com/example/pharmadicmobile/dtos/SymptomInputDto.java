package com.example.pharmadicmobile.dtos;

public class SymptomInputDto {
    private String symptomName;
    private String description;

    public SymptomInputDto() {}

    public String getSymptomName() { return symptomName; }
    public void setSymptomName(String symptomName) { this.symptomName = symptomName; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }
}
