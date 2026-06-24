package com.example.pharmadicmobile.models;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

public class Disease implements Serializable {
    private int diseaseId;
    private String diseaseName;
    private String description;
    private String warningSigns;
    private List<DiseaseMedicine> diseaseMedicines = new ArrayList<>();
    private List<Symptom> symptoms = new ArrayList<>();

    public Disease() {}

    public int getDiseaseId() { return diseaseId; }
    public void setDiseaseId(int diseaseId) { this.diseaseId = diseaseId; }

    public String getDiseaseName() { return diseaseName; }
    public void setDiseaseName(String diseaseName) { this.diseaseName = diseaseName; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public String getWarningSigns() { return warningSigns; }
    public void setWarningSigns(String warningSigns) { this.warningSigns = warningSigns; }

    public List<DiseaseMedicine> getDiseaseMedicines() { return diseaseMedicines; }
    public void setDiseaseMedicines(List<DiseaseMedicine> diseaseMedicines) { this.diseaseMedicines = diseaseMedicines; }

    public List<Symptom> getSymptoms() { return symptoms; }
    public void setSymptoms(List<Symptom> symptoms) { this.symptoms = symptoms; }
}
