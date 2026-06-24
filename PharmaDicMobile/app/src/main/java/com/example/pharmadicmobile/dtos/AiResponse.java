package com.example.pharmadicmobile.dtos;

import com.google.gson.annotations.SerializedName;

public class AiResponse {
    // Hỗ trợ cả "answer" (camelCase) và "Answer" (PascalCase) từ .NET
    @SerializedName(value = "answer", alternate = {"Answer"})
    private String answer;

    @SerializedName(value = "isError", alternate = {"IsError"})
    private boolean isError;

    @SerializedName(value = "error", alternate = {"Error"})
    private String error;

    public AiResponse() {}

    public String getAnswer() { return answer; }
    public void setAnswer(String answer) { this.answer = answer; }

    public boolean isError() { return isError; }
    public void setIsError(boolean isError) { this.isError = isError; }

    public String getError() { return error; }
    public void setError(String error) { this.error = error; }
}
