package com.example.pharmadicmobile.dtos;

import com.google.gson.annotations.SerializedName;

public class LoginRequest {
    @SerializedName("Email")
    private String email;

    @SerializedName("Password")
    private String password;

    public LoginRequest() {}
    public LoginRequest(String email, String password) {
        this.email = email;
        this.password = password;
    }

    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }

    public String getPassword() { return password; }
    public void setPassword(String password) { this.password = password; }
}
