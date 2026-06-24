package com.example.pharmadicmobile.models;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

public class Symptom implements Serializable {
    private int symptomId;
    private String symptomName;
    private String description;
    private List<Disease> diseases = new ArrayList<>();

    public Symptom() {}

    public int getSymptomId() { return symptomId; }
    public void setSymptomId(int symptomId) { this.symptomId = symptomId; }

    public String getSymptomName() { return symptomName; }
    public void setSymptomName(String symptomName) { this.symptomName = symptomName; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public List<Disease> getDiseases() { return diseases; }
    public void setDiseases(List<Disease> diseases) { this.diseases = diseases; }
}
