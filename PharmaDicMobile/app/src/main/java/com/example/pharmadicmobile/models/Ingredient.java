package com.example.pharmadicmobile.models;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

public class Ingredient implements Serializable {
    private int ingredientId;
    private String ingredientName;
    private String description;
    private List<MedicineIngredient> medicineIngredients = new ArrayList<>();

    public Ingredient() {}

    public int getIngredientId() { return ingredientId; }
    public void setIngredientId(int ingredientId) { this.ingredientId = ingredientId; }

    public String getIngredientName() { return ingredientName; }
    public void setIngredientName(String ingredientName) { this.ingredientName = ingredientName; }

    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }

    public List<MedicineIngredient> getMedicineIngredients() { return medicineIngredients; }
    public void setMedicineIngredients(List<MedicineIngredient> medicineIngredients) { this.medicineIngredients = medicineIngredients; }
}
