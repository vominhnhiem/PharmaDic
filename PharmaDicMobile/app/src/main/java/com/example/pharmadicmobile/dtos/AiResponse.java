package com.example.pharmadicmobile.dtos;

import com.google.gson.annotations.SerializedName;

public class AiResponse {
    @SerializedName("Answer")
    private String answer;

    @SerializedName("IsError")
    private boolean isError;

    @SerializedName("Error")
    private String error;

    public AiResponse() {}

    public String getAnswer() { return answer; }
    public void setAnswer(String answer) { this.answer = answer; }

    public boolean isError() { return isError; }
    public void setError(boolean isError) { this.isError = isError; }

    public String getError() { return error; }
    public void setError(String error) { this.error = error; }
}
