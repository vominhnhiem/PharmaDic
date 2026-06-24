package com.example.pharmadicmobile.dtos;

public class AiResponse {
    private String answer;
    private boolean isError;
    private String error;

    public AiResponse() {}

    public String getAnswer() { return answer; }
    public void setAnswer(String answer) { this.answer = answer; }

    public boolean isError() { return isError; }
    public void setError(boolean error) { isError = error; }

    public String getError() { return error; }
    public void setError(String error) { this.error = error; }
}
