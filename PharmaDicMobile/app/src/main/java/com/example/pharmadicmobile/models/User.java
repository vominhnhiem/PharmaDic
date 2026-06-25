package com.example.pharmadicmobile.models;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class User implements Serializable {
    private int userId;
    private String fullName;
    private String email;
    private String passwordHash;
    private String role;
    private Date createdAt;
    private List<Favorite> favorites = new ArrayList<>();
    private List<SearchHistory> searchHistories = new ArrayList<>();

    public User() {}

    public int getUserId() { return userId; }
    public void setUserId(int userId) { this.userId = userId; }

    public String getFullName() { return fullName; }
    public void setFullName(String fullName) { this.fullName = fullName; }

    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }

    public String getPasswordHash() { return passwordHash; }
    public void setPasswordHash(String passwordHash) { this.passwordHash = passwordHash; }

    public String getRole() { return role; }
    public void setRole(String role) { this.role = role; }

    public Date getCreatedAt() { return createdAt; }
    public void setCreatedAt(Date createdAt) { this.createdAt = createdAt; }

    public List<Favorite> getFavorites() { return favorites; }
    public void setFavorites(List<Favorite> favorites) { this.favorites = favorites; }

    public List<SearchHistory> getSearchHistories() { return searchHistories; }
    public void setSearchHistories(List<SearchHistory> searchHistories) { this.searchHistories = searchHistories; }
}
