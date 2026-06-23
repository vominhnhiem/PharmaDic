package com.example.pharmadicmobile;

import android.os.Bundle;
import android.widget.LinearLayout;
import androidx.appcompat.app.AppCompatActivity;

public class HistoryActivity extends AppCompatActivity {

    private LinearLayout navHome;
    private LinearLayout navSearch;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_history);

        navHome = findViewById(R.id.navHome);
        navSearch = findViewById(R.id.navSearch);

        // Quay lại trang chủ
        navHome.setOnClickListener(v -> {
            finish();
        });

        // Chuyển sang trang tra cứu
        navSearch.setOnClickListener(v -> {
            // Có thể mở SymptomSearchActivity ở đây
            finish();
        });
    }
}