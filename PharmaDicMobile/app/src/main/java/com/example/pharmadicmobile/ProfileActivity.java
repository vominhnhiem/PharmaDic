package com.example.pharmadicmobile;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;
import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class ProfileActivity extends AppCompatActivity {

    private LinearLayout navHome, navSearch, navAI, navProfile;
    private TextView tvProfileName, tvProfileEmail, tvProfileRole;
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
        btnLogout = findViewById(R.id.btnLogout);
    }

    private void loadUserData() {
        SharedPreferences sharedPreferences = getSharedPreferences("PharmaDic", Context.MODE_PRIVATE);
        // Lưu ý: Trong thực tế bạn có thể gọi thêm API lấy Profile hoặc giải mã JWT token để lấy tên.
        // Ở đây mình tạm hiển thị Role đã lưu.
        String role = sharedPreferences.getString("role", "User");
        tvProfileRole.setText(role.equals("Admin") ? "Quản trị viên" : "Dược sĩ / Người dùng");
        
        // Mock email nếu chưa lưu
        String email = sharedPreferences.getString("email", "pharmacist@pharmadic.vn");
        tvProfileEmail.setText(email);
    }

    private void setupNavigation() {
        navHome.setOnClickListener(v -> {
            Intent intent = new Intent(ProfileActivity.this, HomeActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
            startActivity(intent);
            finish();
        });

        navSearch.setOnClickListener(v -> {
            startActivity(new Intent(ProfileActivity.this, SymptomSearchActivity.class));
            finish();
        });

        navAI.setOnClickListener(v -> {
            startActivity(new Intent(ProfileActivity.this, ChatAIActivity.class));
            finish();
        });
        
        // navProfile đã ở đây rồi
    }

    private void performLogout() {
        SharedPreferences sharedPreferences = getSharedPreferences("PharmaDic", Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.clear(); // Xóa sạch token và thông tin login
        editor.apply();

        Intent intent = new Intent(ProfileActivity.this, MainActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        startActivity(intent);
        finish();
    }
}
