package com.example.pharmadicmobile.dtos;

public class UserRegistrationTrendDto {
    private String timeLabel;
    private int userCount;

    public UserRegistrationTrendDto() {}

    public String getTimeLabel() { return timeLabel; }
    public void setTimeLabel(String timeLabel) { this.timeLabel = timeLabel; }

    public int getUserCount() { return userCount; }
    public void setUserCount(int userCount) { this.userCount = userCount; }
}
