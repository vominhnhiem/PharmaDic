package com.example.pharmadicmobile;

import android.content.Intent;
import android.os.Bundle;
import android.widget.LinearLayout;
import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.cardview.widget.CardView;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class HomeActivity extends AppCompatActivity {

    private LinearLayout btnHeroSearch;
    private LinearLayout btnHistory;
    private LinearLayout btnAskAI;
    private LinearLayout navSearch;
    private LinearLayout navAI;
    private LinearLayout navProfile;
    private CardView cardMedicine;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_home);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(android.R.id.content), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        btnHeroSearch = findViewById(R.id.btnHeroSearch);
        btnHistory = findViewById(R.id.btnHistory);
        btnAskAI = findViewById(R.id.btnAskAI);
        navSearch = findViewById(R.id.navSearch);
        navAI = findViewById(R.id.navAI);
        navProfile = findViewById(R.id.navProfile);
        cardMedicine = findViewById(R.id.cardMedicine);

        // Chuyển sang trang tra cứu triệu chứng khi bấm vào nút chính
        btnHeroSearch.setOnClickListener(v -> {
            Intent intent = new Intent(HomeActivity.this, SymptomSearchActivity.class);
            startActivity(intent);
        });

        // Chuyển sang trang lịch sử tra cứu
        btnHistory.setOnClickListener(v -> {
            Intent intent = new Intent(HomeActivity.this, HistoryActivity.class);
            startActivity(intent);
        });

        // Chuyển sang trang tra cứu triệu chứng khi bấm vào mục "Tra cứu" ở Bottom Nav
        navSearch.setOnClickListener(v -> {
            Intent intent = new Intent(HomeActivity.this, SymptomSearchActivity.class);
            startActivity(intent);
        });

        // Chuyển sang trang Trợ lý AI
        btnAskAI.setOnClickListener(v -> {
            Intent intent = new Intent(HomeActivity.this, ChatAIActivity.class);
            startActivity(intent);
        });

        navAI.setOnClickListener(v -> {
            Intent intent = new Intent(HomeActivity.this, ChatAIActivity.class);
            startActivity(intent);
        });

        // Chuyển sang trang Tài khoản (Profile)
        navProfile.setOnClickListener(v -> {
            Intent intent = new Intent(HomeActivity.this, ProfileActivity.class);
            startActivity(intent);
        });

        // Chuyển sang trang Chi tiết thuốc khi bấm vào card thuốc
        if (cardMedicine != null) {
            cardMedicine.setOnClickListener(v -> {
                Intent intent = new Intent(HomeActivity.this, MedicineDetailActivity.class);
                startActivity(intent);
            });
        }
    }
}