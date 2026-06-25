package com.example.pharmadicmobile;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.pharmadicmobile.api.RetrofitClient;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ProfileActivity extends AppCompatActivity {

    private LinearLayout navHome, navSearch, navAI, navProfile;
    private TextView tvProfileName, tvProfileEmail, tvProfileRole;
    private TextView tvSearchCount, tvReportCount;
    private Button btnLogout;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_profile);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        initViews();
        loadUserData();
        fetchStats();
        setupNavigation();

        btnLogout.setOnClickListener(v -> performLogout());
    }

    private void initViews() {
        navHome = findViewById(R.id.navHome);
        navSearch = findViewById(R.id.navSearch);
        navAI = findViewById(R.id.navAI);
        navProfile = findViewById(R.id.navProfile);
        
        tvProfileName = findViewById(R.id.tvProfileName);
        tvProfileEmail = findViewById(R.id.tvProfileEmail);
        tvProfileRole = findViewById(R.id.tvProfileRole);
        
        // Giả sử bạn có ID này trong XML cho các con số
        // Nếu chưa có, code vẫn chạy nhưng không cập nhật được số
        tvSearchCount = findViewById(android.R.id.text1); // Cần kiểm tra ID thực tế trong layout
        tvReportCount = findViewById(android.R.id.text2); 
        
        btnLogout = findViewById(R.id.btnLogout);
    }
/// log
/// lỗi AI
/// đã fix thành công
    private void loadUserData() {
        SharedPreferences sharedPreferences = getSharedPreferences("PharmaDic", Context.MODE_PRIVATE);

        // Lấy dữ liệu từ SharedPreferences, nếu không có sẽ để trống thay vì dùng text cứng
        String fullName = sharedPreferences.getString("fullName", "");
        String email = sharedPreferences.getString("email", "");
        String role = sharedPreferences.getString("role", "User");

        if (!fullName.isEmpty()) {
            tvProfileName.setText(fullName);
        } else {
            tvProfileName.setText("Chưa cập nhật tên");
        }

        if (!email.isEmpty()) {
            tvProfileEmail.setText(email);
        } else {
            tvProfileEmail.setText("Chưa cập nhật email");
        }

        // Cập nhật hiển thị Role
        if ("Admin".equalsIgnoreCase(role)) {
            tvProfileRole.setText("Quản trị viên");
        } else if ("Pharmacist".equalsIgnoreCase(role) || "Dược sĩ".equalsIgnoreCase(role)) {
            tvProfileRole.setText("Dược sĩ chuyên môn");
        } else {
            tvProfileRole.setText("Người dùng / Nhân viên");
        }
    }

    private void fetchStats() {
        SharedPreferences sharedPreferences = getSharedPreferences("PharmaDic", Context.MODE_PRIVATE);
        String token = sharedPreferences.getString("token", "");
        int userId = sharedPreferences.getInt("userId", 0);

        if (token.isEmpty() || userId == 0) return;

        String authHeader = "Bearer " + token;

        // Lấy số lượng lịch sử Chat AI (Báo cáo)
        RetrofitClient.getApiService().getChatHistory(authHeader, userId).enqueue(new Callback<List<Object>>() {
            @Override
            public void onResponse(@NonNull Call<List<Object>> call, @NonNull Response<List<Object>> response) {
                if (response.isSuccessful() && response.body() != null) {
                    int count = response.body().size();
                    // Cập nhật số lượng vào UI (Tìm đúng TextView theo cấu trúc layout của bạn)
                    // Ở đây tôi tìm theo ví dụ, bạn nên đặt ID cho TextView số 48 trong XML
                }
            }

            @Override
            public void onFailure(@NonNull Call<List<Object>> call, @NonNull Throwable t) {}
        });
    }

    private void setupNavigation() {
        navHome.setOnClickListener(v -> {
            Intent intent = new Intent(this, HomeActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
            startActivity(intent);
        });

        navSearch.setOnClickListener(v -> {
            startActivity(new Intent(this, SymptomSearchActivity.class));
        });

        navAI.setOnClickListener(v -> {
            startActivity(new Intent(this, ChatAIActivity.class));
        });
    }

    private void performLogout() {
        SharedPreferences sharedPreferences = getSharedPreferences("PharmaDic", Context.MODE_PRIVATE);
        sharedPreferences.edit().clear().apply();

        Toast.makeText(this, "Đã đăng xuất", Toast.LENGTH_SHORT).show();

        Intent intent = new Intent(ProfileActivity.this, MainActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        startActivity(intent);
        finish();
    }
}
