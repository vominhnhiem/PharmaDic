package com.example.pharmadicmobile.dtos;

public class TokenResponse {
    private String message;
    private String token;
    private String role;

    public TokenResponse() {}

    public String getMessage() { return message; }
    public void setMessage(String message) { this.message = message; }

    public String getToken() { return token; }
    public void setToken(String token) { this.token = token; }

    public String getRole() { return role; }
    public void setRole(String role) { this.role = role; }
}
