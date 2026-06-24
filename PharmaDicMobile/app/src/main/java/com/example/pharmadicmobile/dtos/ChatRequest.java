package com.example.pharmadicmobile.dtos;

public class ChatRequest {
    private int userId;
    private String question;

    public ChatRequest() {}

    public ChatRequest(String question) {
        this.question = question;
    }
    
    public ChatRequest(int userId, String question) {
        this.userId = userId;
        this.question = question;
    }

    public int getUserId() {
        return userId;
    }

    public void setUserId(int userId) {
        this.userId = userId;
    }

    public String getQuestion() {
        return question;
    }

    public void setQuestion(String question) {
        this.question = question;
    }
}
