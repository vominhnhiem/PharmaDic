package com.example.pharmadicmobile.models;

public class ChatMessage {
    private String content;
    private boolean isUser;
    private String time;

    public ChatMessage(String content, boolean isUser, String time) {
        this.content = content;
        this.isUser = isUser;
        this.time = time;
    }

    public String getContent() { return content; }
    public boolean isUser() { return isUser; }
    public String getTime() { return time; }
}
