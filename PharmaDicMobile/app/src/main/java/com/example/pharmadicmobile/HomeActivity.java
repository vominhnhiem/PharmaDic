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

    private LinearLayout btnHeroSearch, btnHistory, btnAskAI;
    private LinearLayout navSearch, navAI, navProfile;
    private CardView cardMedicine;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        // Bật chế độ hiển thị tràn viền
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_home);

        // Xử lý khoảng cách với thanh trạng thái và điều hướng
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(android.R.id.content), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        // Ánh xạ các View từ Layout
        initViews();

        // Thiết lập các sự kiện click chuyển trang
        setupClickListeners();
    }

    /**
     * Khởi tạo các thành phần giao diện
     */
    private void initViews() {
        btnHeroSearch = findViewById(R.id.btnHeroSearch);
        btnHistory = findViewById(R.id.btnHistory);
        btnAskAI = findViewById(R.id.btnAskAI);
        navSearch = findViewById(R.id.navSearch);
        navAI = findViewById(R.id.navAI);
        navProfile = findViewById(R.id.navProfile);
        cardMedicine = findViewById(R.id.cardMedicine);
    }

    /**
     * Cấu hình xử lý khi người dùng nhấn vào các nút
     */
    private void setupClickListeners() {
        // Chuyển sang Tra cứu triệu chứng
        btnHeroSearch.setOnClickListener(v -> startActivity(new Intent(this, SymptomSearchActivity.class)));
        navSearch.setOnClickListener(v -> startActivity(new Intent(this, SymptomSearchActivity.class)));

        // Xem lịch sử tra cứu
        btnHistory.setOnClickListener(v -> startActivity(new Intent(this, HistoryActivity.class)));

        // Mở trợ lý AI
        btnAskAI.setOnClickListener(v -> startActivity(new Intent(this, ChatAIActivity.class)));
        navAI.setOnClickListener(v -> startActivity(new Intent(this, ChatAIActivity.class)));

        // Vào trang cá nhân
        navProfile.setOnClickListener(v -> startActivity(new Intent(this, ProfileActivity.class)));

        // Xem chi tiết một loại thuốc
        if (cardMedicine != null) {
            cardMedicine.setOnClickListener(v -> startActivity(new Intent(this, MedicineDetailActivity.class)));
        }
    }
}