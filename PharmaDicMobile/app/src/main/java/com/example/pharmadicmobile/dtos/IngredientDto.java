package com.example.pharmadicmobile.dtos;

import com.google.gson.annotations.SerializedName;

public class IngredientDto {
    @SerializedName(value = "IngredientName", alternate = {"ingredientName"})
    private String ingredientName;
    
    @SerializedName(value = "Amount", alternate = {"amount"})
    private String amount;

    public IngredientDto() {}

    public String getIngredientName() { return ingredientName; }
    public void setIngredientName(String ingredientName) { this.ingredientName = ingredientName; }

    public String getAmount() { return amount; }
    public void setAmount(String amount) { this.amount = amount; }
}
