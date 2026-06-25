package com.example.pharmadicmobile.dtos;

public class MedicineIngredientInputDto {
    private int ingredientId;
    private String amount;

    public MedicineIngredientInputDto() {}

    public int getIngredientId() { return ingredientId; }
    public void setIngredientId(int ingredientId) { this.ingredientId = ingredientId; }

    public String getAmount() { return amount; }
    public void setAmount(String amount) { this.amount = amount; }
}
