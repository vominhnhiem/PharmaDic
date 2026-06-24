package com.example.pharmadicmobile;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.pharmadicmobile.api.RetrofitClient;
import com.example.pharmadicmobile.dtos.LoginRequest;
import com.example.pharmadicmobile.dtos.TokenResponse;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends AppCompatActivity {

    private LinearLayout tabLogin, tabRegister;
    private TextView tvTabLogin, tvTabRegister;
    private View indicatorLogin, indicatorRegister;
    private LinearLayout formLogin, formRegister;
    private Button btnStart;
    private EditText etEmail, etPassword;

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

        // Kiểm tra nếu đã có token thì vào thẳng Home
        checkLoggedIn();

        initViews();
        setupTabs();
        setupLoginAction();
    }

    private void checkLoggedIn() {
        SharedPreferences sharedPreferences = getSharedPreferences("PharmaDic", Context.MODE_PRIVATE);
        String token = sharedPreferences.getString("token", null);
        if (token != null) {
            navigateToHome();
        }
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
        
        etEmail = findViewById(R.id.etEmail);
        etPassword = findViewById(R.id.etPassword);
    }

    private void setupTabs() {
        tabLogin.setOnClickListener(v -> switchTab(true));
        tabRegister.setOnClickListener(v -> switchTab(false));
    }

    private void setupLoginAction() {
        btnStart.setOnClickListener(v -> {
            String email = etEmail.getText().toString().trim();
            String password = etPassword.getText().toString().trim();

            if (email.isEmpty() || password.isEmpty()) {
                Toast.makeText(this, "Vui lòng nhập đầy đủ thông tin", Toast.LENGTH_SHORT).show();
                return;
            }

            performLogin(email, password);
        });
    }

    private void performLogin(String email, String password) {
        btnStart.setEnabled(false);
        btnStart.setText("Đang đăng nhập...");

        LoginRequest loginRequest = new LoginRequest(email, password);
        RetrofitClient.getApiService().login(loginRequest).enqueue(new Callback<TokenResponse>() {
            @Override
            public void onResponse(Call<TokenResponse> call, Response<TokenResponse> response) {
                btnStart.setEnabled(true);
                btnStart.setText("Bắt đầu");

                if (response.isSuccessful() && response.body() != null) {
                    TokenResponse tokenResponse = response.body();
                    saveAuthData(tokenResponse.getToken(), tokenResponse.getRole());
                    Toast.makeText(MainActivity.this, "Đăng nhập thành công!", Toast.LENGTH_SHORT).show();
                    navigateToHome();
                } else {
                    Toast.makeText(MainActivity.this, "Email hoặc mật khẩu không đúng", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<TokenResponse> call, Throwable t) {
                btnStart.setEnabled(true);
                btnStart.setText("Bắt đầu");
                Toast.makeText(MainActivity.this, "Lỗi kết nối: " + t.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void saveAuthData(String token, String role) {
        SharedPreferences sharedPreferences = getSharedPreferences("PharmaDic", Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.putString("token", token);
        editor.putString("role", role);
        editor.apply();
    }

    private void navigateToHome() {
        Intent intent = new Intent(MainActivity.this, HomeActivity.class);
        startActivity(intent);
        finish();
    }

    private void switchTab(boolean isLogin) {
        if (isLogin) {
            tvTabLogin.setTextColor(ContextCompat.getColor(this, R.color.primary));
            indicatorLogin.setVisibility(View.VISIBLE);
            formLogin.setVisibility(View.VISIBLE);
            tvTabRegister.setTextColor(ContextCompat.getColor(this, R.color.on_surface_variant));
            indicatorRegister.setVisibility(View.INVISIBLE);
            formRegister.setVisibility(View.GONE);
        } else {
            tvTabRegister.setTextColor(ContextCompat.getColor(this, R.color.primary));
            indicatorRegister.setVisibility(View.VISIBLE);
            formRegister.setVisibility(View.VISIBLE);
            tvTabLogin.setTextColor(ContextCompat.getColor(this, R.color.on_surface_variant));
            indicatorLogin.setVisibility(View.INVISIBLE);
            formLogin.setVisibility(View.GONE);
        }
    }
}
