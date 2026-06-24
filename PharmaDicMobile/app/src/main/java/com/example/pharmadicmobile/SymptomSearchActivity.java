package com.example.pharmadicmobile;

import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;
import android.widget.LinearLayout;
import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class SymptomSearchActivity extends AppCompatActivity {

    private Button btnSearchSymptoms;
    private LinearLayout navHome;
    private LinearLayout navAI;
    private LinearLayout navHistory;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_symptom_search);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(android.R.id.content), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        btnSearchSymptoms = findViewById(R.id.btnSearchSymptoms);
        navHome = findViewById(R.id.navHome);
        navAI = findViewById(R.id.navAI);
        navHistory = findViewById(R.id.navHistory);

        btnSearchSymptoms.setOnClickListener(v -> {
            // Chuyển sang màn hình Kết quả gợi ý thuốc
            Intent intent = new Intent(SymptomSearchActivity.this, SearchResultsActivity.class);
            startActivity(intent);
        });

        // Quay lại trang chủ
        navHome.setOnClickListener(v -> {
            finish();
        });

        // Chuyển sang trang Trợ lý AI
        navAI.setOnClickListener(v -> {
            Intent intent = new Intent(SymptomSearchActivity.this, ChatAIActivity.class);
            startActivity(intent);
            finish();
        });

        // Chuyển sang trang Lịch sử
        navHistory.setOnClickListener(v -> {
            Intent intent = new Intent(SymptomSearchActivity.this, HistoryActivity.class);
            startActivity(intent);
            finish();
        });
    }
}