package com.example.pharmadicmobile.models;

import java.io.Serializable;

public class MedicineIngredient implements Serializable {
    private int medicineId;
    private int ingredientId;
    private String amount;
    private Ingredient ingredient;
    private Medicine medicine;

    public MedicineIngredient() {}

    public int getMedicineId() { return medicineId; }
    public void setMedicineId(int medicineId) { this.medicineId = medicineId; }

    public int getIngredientId() { return ingredientId; }
    public void setIngredientId(int ingredientId) { this.ingredientId = ingredientId; }

    public String getAmount() { return amount; }
    public void setAmount(String amount) { this.amount = amount; }

    public Ingredient getIngredient() { return ingredient; }
    public void setIngredient(Ingredient ingredient) { this.ingredient = ingredient; }

    public Medicine getMedicine() { return medicine; }
    public void setMedicine(Medicine medicine) { this.medicine = medicine; }
}
