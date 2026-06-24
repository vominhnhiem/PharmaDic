package com.example.pharmadicmobile;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
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

        // Tự động đăng nhập nếu đã có token
        checkLoggedIn();

        initViews();
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
        btnStart = findViewById(R.id.btnStart);
        etEmail = findViewById(R.id.etEmail);
        etPassword = findViewById(R.id.etPassword);
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
            public void onResponse(@NonNull Call<TokenResponse> call, @NonNull Response<TokenResponse> response) {
                btnStart.setEnabled(true);
                btnStart.setText("Bắt đầu →");

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
            public void onFailure(@NonNull Call<TokenResponse> call, @NonNull Throwable t) {
                btnStart.setEnabled(true);
                btnStart.setText("Bắt đầu →");
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
}
