package com.example.pharmadicmobile;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class MainActivity extends AppCompatActivity {

    private LinearLayout tabLogin, tabRegister;
    private TextView tvTabLogin, tvTabRegister;
    private View indicatorLogin, indicatorRegister;
    private LinearLayout formLogin, formRegister;
    private Button btnStart;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);
        
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        initViews();
        setupTabs();
        setupLoginAction();
    }

    private void initViews() {
        tabLogin = findViewById(R.id.tabLogin);
        tabRegister = findViewById(R.id.tabRegister);
        tvTabLogin = findViewById(R.id.tvTabLogin);
        tvTabRegister = findViewById(R.id.tvTabRegister);
        indicatorLogin = findViewById(R.id.indicatorLogin);
        indicatorRegister = findViewById(R.id.indicatorRegister);
        formLogin = findViewById(R.id.formLogin);
        formRegister = findViewById(R.id.formRegister);
        btnStart = findViewById(R.id.btnStart);
    }

    private void setupTabs() {
        tabLogin.setOnClickListener(v -> switchTab(true));
        tabRegister.setOnClickListener(v -> switchTab(false));
    }

    private void setupLoginAction() {
        btnStart.setOnClickListener(v -> {
            // Chuyển sang trang HomeActivity
            Intent intent = new Intent(MainActivity.this, HomeActivity.class);
            startActivity(intent);
            // Có thể dùng finish() nếu không muốn quay lại màn hình đăng nhập khi nhấn Back
            // finish(); 
        });
    }

    private void switchTab(boolean isLogin) {
        if (isLogin) {
            // Active Login
            tvTabLogin.setTextColor(ContextCompat.getColor(this, R.color.primary));
            indicatorLogin.setVisibility(View.VISIBLE);
            formLogin.setVisibility(View.VISIBLE);

            // Inactive Register
            tvTabRegister.setTextColor(ContextCompat.getColor(this, R.color.on_surface_variant));
            indicatorRegister.setVisibility(View.INVISIBLE);
            formRegister.setVisibility(View.GONE);
        } else {
            // Active Register
            tvTabRegister.setTextColor(ContextCompat.getColor(this, R.color.primary));
            indicatorRegister.setVisibility(View.VISIBLE);
            formRegister.setVisibility(View.VISIBLE);

            // Inactive Login
            tvTabLogin.setTextColor(ContextCompat.getColor(this, R.color.on_surface_variant));
            indicatorLogin.setVisibility(View.INVISIBLE);
            formLogin.setVisibility(View.GONE);
        }
    }
}