package com.example.pharmadicmobile;

import android.content.Intent;
import android.os.Bundle;
import android.widget.LinearLayout;
import com.google.android.material.button.MaterialButton;
import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class HistoryActivity extends AppCompatActivity {

    private LinearLayout navHome;
    private LinearLayout navSearch;
    private LinearLayout navAI;
    private MaterialButton btnSearch1;
    private MaterialButton btnSearch2;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_history);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(android.R.id.content), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        navHome = findViewById(R.id.navHome);
        navSearch = findViewById(R.id.navSearch);
        navAI = findViewById(R.id.navAI);
        btnSearch1 = findViewById(R.id.btnSearch1);
        btnSearch2 = findViewById(R.id.btnSearch2);

        navHome.setOnClickListener(v -> {
            Intent intent = new Intent(HistoryActivity.this, HomeActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
            startActivity(intent);
            finish();
        });

        navSearch.setOnClickListener(v -> {
            Intent intent = new Intent(HistoryActivity.this, SymptomSearchActivity.class);
            startActivity(intent);
            finish();
        });

        navAI.setOnClickListener(v -> {
            Intent intent = new Intent(HistoryActivity.this, ChatAIActivity.class);
            startActivity(intent);
            finish();
        });

        // Mở chi tiết thuốc khi bấm vào nút tìm kiếm trong lịch sử
        btnSearch1.setOnClickListener(v -> {
            Intent intent = new Intent(HistoryActivity.this, MedicineDetailActivity.class);
            startActivity(intent);
        });

        btnSearch2.setOnClickListener(v -> {
            Intent intent = new Intent(HistoryActivity.this, MedicineDetailActivity.class);
            startActivity(intent);
        });
    }
}