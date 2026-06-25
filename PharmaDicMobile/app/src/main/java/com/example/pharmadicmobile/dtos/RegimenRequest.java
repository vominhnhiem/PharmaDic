package com.example.pharmadicmobile.dtos;

public class RegimenRequest {
    private String symptoms;

    public RegimenRequest() {}
    public RegimenRequest(String symptoms) { this.symptoms = symptoms; }

    public String getSymptoms() { return symptoms; }
    public void setSymptoms(String symptoms) { this.symptoms = symptoms; }
}
