package com.example.pharmadicmobile.models;

import java.io.Serializable;
import java.util.Date;

public class SearchHistory implements Serializable {
    private int searchId;
    private Integer userId;
    private String keyword;
    private String searchType;
    private Date searchDate;
    private User user;

    public SearchHistory() {}

    public int getSearchId() { return searchId; }
    public void setSearchId(int searchId) { this.searchId = searchId; }

    public Integer getUserId() { return userId; }
    public void setUserId(Integer userId) { this.userId = userId; }

    public String getKeyword() { return keyword; }
    public void setKeyword(String keyword) { this.keyword = keyword; }

    public String getSearchType() { return searchType; }
    public void setSearchType(String searchType) { this.searchType = searchType; }

    public Date getSearchDate() { return searchDate; }
    public void setSearchDate(Date searchDate) { this.searchDate = searchDate; }

    public User getUser() { return user; }
    public void setUser(User user) { this.user = user; }
}
