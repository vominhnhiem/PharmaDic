package com.example.pharmadicmobile.models;

import java.io.Serializable;
import java.util.Date;

public class Favorite implements Serializable {
    private int favoriteId;
    private int userId;
    private int medicineId;
    private Date createdAt;
    private Medicine medicine;
    private User user;

    public Favorite() {}

    public int getFavoriteId() { return favoriteId; }
    public void setFavoriteId(int favoriteId) { this.favoriteId = favoriteId; }

    public int getUserId() { return userId; }
    public void setUserId(int userId) { this.userId = userId; }

    public int getMedicineId() { return medicineId; }
    public void setMedicineId(int medicineId) { this.medicineId = medicineId; }

    public Date getCreatedAt() { return createdAt; }
    public void setCreatedAt(Date createdAt) { this.createdAt = createdAt; }

    public Medicine getMedicine() { return medicine; }
    public void setMedicine(Medicine medicine) { this.medicine = medicine; }

    public User getUser() { return user; }
    public void setUser(User user) { this.user = user; }
}
