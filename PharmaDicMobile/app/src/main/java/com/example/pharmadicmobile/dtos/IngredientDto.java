package com.example.pharmadicmobile.dtos;

public class IngredientDto {
    private String ingredientName;
    private String amount;

    public IngredientDto() {}

    public String getIngredientName() { return ingredientName; }
    public void setIngredientName(String ingredientName) { this.ingredientName = ingredientName; }

    public String getAmount() { return amount; }
    public void setAmount(String amount) { this.amount = amount; }
}
