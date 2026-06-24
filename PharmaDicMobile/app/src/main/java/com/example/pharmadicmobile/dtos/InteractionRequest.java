package com.example.pharmadicmobile.dtos;

import java.util.ArrayList;
import java.util.List;

public class InteractionRequest {
    private List<Integer> medicineIds = new ArrayList<>();

    public InteractionRequest() {}
    public InteractionRequest(List<Integer> medicineIds) { this.medicineIds = medicineIds; }

    public List<Integer> getMedicineIds() { return medicineIds; }
    public void setMedicineIds(List<Integer> medicineIds) { this.medicineIds = medicineIds; }
}
