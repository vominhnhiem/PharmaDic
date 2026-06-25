package com.example.pharmadicmobile.dtos;

import com.google.gson.annotations.SerializedName;

public class TokenResponse {
    private String message;
    private String token;
    private String role;
    
    @SerializedName(value = "userId", alternate = {"UserId"})
    private int userId;
    
    @SerializedName(value = "fullName", alternate = {"FullName"})
    private String fullName;
    
    @SerializedName(value = "email", alternate = {"Email"})
    private String email;

    public TokenResponse() {}

    public String getMessage() { return message; }
    public void setMessage(String message) { this.message = message; }

    public String getToken() { return token; }
    public void setToken(String token) { this.token = token; }

    public String getRole() { return role; }
    public void setRole(String role) { this.role = role; }

    public int getUserId() { return userId; }
    public void setUserId(int userId) { this.userId = userId; }

    public String getFullName() { return fullName; }
    public void setFullName(String fullName) { this.fullName = fullName; }

    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
}
