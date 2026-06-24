package com.example.pharmadicmobile;

import android.content.Intent;
import android.os.Bundle;
import android.widget.ImageView;
import android.widget.LinearLayout;
import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class ChatAIActivity extends AppCompatActivity {

    private LinearLayout navHome, navSearch;
    private ImageView btnBack;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        // Bật chế độ tràn viền
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_chat_ai);

        // Xử lý khoảng cách hệ thống (Status bar/Navigation bar)
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(android.R.id.content), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        initViews();
        setupClickListeners();
    }

    /**
     * Khởi tạo các thành phần giao diện
     */
    private void initViews() {
        btnBack = findViewById(R.id.btnBack);
        navHome = findViewById(R.id.navHome);
        navSearch = findViewById(R.id.navSearch);
    }

    /**
     * Thiết lập các sự kiện click
     */
    private void setupClickListeners() {
        // Nút quay lại trang trước đó
        btnBack.setOnClickListener(v -> finish());

        // Chuyển về trang chủ
        navHome.setOnClickListener(v -> {
            Intent intent = new Intent(this, HomeActivity.class);
            // Xóa các Activity đè lên trang chủ nếu có
            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
            startActivity(intent);
            finish();
        });

        // Chuyển sang trang Tra cứu triệu chứng
        navSearch.setOnClickListener(v -> {
            startActivity(new Intent(this, SymptomSearchActivity.class));
            finish();
        });
    }
}