package com.example.pharmadicmobile;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.pharmadicmobile.api.RetrofitClient;
import com.example.pharmadicmobile.dtos.IngredientDto;
import com.example.pharmadicmobile.dtos.MedicineDetailDto;
import com.google.android.material.button.MaterialButton;

import java.util.stream.Collectors;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MedicineDetailActivity extends AppCompatActivity {

    private int medicineId;
    private TextView tvMedicineName, tvIngredientsSummary, tvManufacturerDetail, tvDosageForm, 
            tvCategoryDetail, tvUses, tvDosage, tvContraindications, tvSideEffects, tvNote;
    private ImageView ivMedicineLarge;
    private MaterialButton btnAskAI;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_medicine_detail);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.toolbar).getParent() instanceof View ? (View)findViewById(R.id.toolbar).getParent() : findViewById(android.R.id.content), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        medicineId = getIntent().getIntExtra("MEDICINE_ID", -1);
        if (medicineId == -1) {
            Toast.makeText(this, "Không tìm thấy thông tin thuốc", Toast.LENGTH_SHORT).show();
            finish();
            return;
        }

        initViews();
        fetchMedicineDetail();
    }

    private void initViews() {
        ivMedicineLarge = findViewById(R.id.ivMedicineLarge);
        tvMedicineName = findViewById(R.id.tvMedicineName);
        tvIngredientsSummary = findViewById(R.id.tvIngredientsSummary);
        tvManufacturerDetail = findViewById(R.id.tvManufacturerDetail);
        tvDosageForm = findViewById(R.id.tvDosageForm);
        tvCategoryDetail = findViewById(R.id.tvCategoryDetail);
        tvUses = findViewById(R.id.tvUses);
        tvDosage = findViewById(R.id.tvDosage);
        tvContraindications = findViewById(R.id.tvContraindications);
        tvSideEffects = findViewById(R.id.tvSideEffects);
        tvNote = findViewById(R.id.tvNote);
        btnAskAI = findViewById(R.id.btnAskAI);

        findViewById(R.id.btnBack).setOnClickListener(v -> finish());
    }

    private void fetchMedicineDetail() {
        RetrofitClient.getApiService().getMedicineDetail(medicineId).enqueue(new Callback<MedicineDetailDto>() {
            @Override
            public void onResponse(@NonNull Call<MedicineDetailDto> call, @NonNull Response<MedicineDetailDto> response) {
                if (response.isSuccessful() && response.body() != null) {
                    displayMedicineDetail(response.body());
                } else {
                    Toast.makeText(MedicineDetailActivity.this, "Lỗi khi lấy dữ liệu chi tiết", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(@NonNull Call<MedicineDetailDto> call, @NonNull Throwable t) {
                Toast.makeText(MedicineDetailActivity.this, "Lỗi kết nối: " + t.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void displayMedicineDetail(MedicineDetailDto medicine) {
        tvMedicineName.setText(medicine.getMedicineName());
        
        // Hiển thị tóm tắt thành phần
        if (medicine.getIngredients() != null && !medicine.getIngredients().isEmpty()) {
            String ingredients = medicine.getIngredients().stream()
                    .map(i -> i.getIngredientName() + " " + (i.getAmount() != null ? i.getAmount() : ""))
                    .collect(Collectors.joining(", "));
            tvIngredientsSummary.setText(ingredients);
        } else {
            tvIngredientsSummary.setText("Chưa rõ thành phần");
        }

        tvManufacturerDetail.setText(medicine.getManufacturer());
        tvDosageForm.setText(medicine.getDosageForm() != null ? medicine.getDosageForm() : "N/A");
        tvCategoryDetail.setText(medicine.getCategoryName() != null ? medicine.getCategoryName() : "Chưa phân loại");
        
        tvUses.setText(medicine.getUses() != null ? medicine.getUses() : "Đang cập nhật...");
        tvDosage.setText(medicine.getDosage() != null ? medicine.getDosage() : "Chưa có thông tin liều dùng");
        tvContraindications.setText(medicine.getContraindications() != null ? medicine.getContraindications() : "Không có");
        tvSideEffects.setText(medicine.getSideEffects() != null ? medicine.getSideEffects() : "Hiếm gặp hoặc chưa ghi nhận");

        btnAskAI.setOnClickListener(v -> {
            Intent intent = new Intent(this, ChatAIActivity.class);
            intent.putExtra("INITIAL_QUERY", "Tôi muốn hỏi về thuốc " + medicine.getMedicineName());
            startActivity(intent);
        });
    }
}
