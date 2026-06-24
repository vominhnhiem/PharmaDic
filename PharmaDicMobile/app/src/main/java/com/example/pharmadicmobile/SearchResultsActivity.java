package com.example.pharmadicmobile;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.TextView;

import androidx.activity.EdgeToEdge;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.pharmadicmobile.adapters.MedicineAdapter;
import com.example.pharmadicmobile.api.RetrofitClient;
import com.example.pharmadicmobile.dtos.MedicineDto;
import com.google.android.material.button.MaterialButton;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class SearchResultsActivity extends AppCompatActivity {

    private TextView tvSearchQuery, tvNoResults;
    private ProgressBar progressBar;
    private RecyclerView rvMedicines;
    private MedicineAdapter adapter;
    private List<MedicineDto> medicineList = new ArrayList<>();
    private String query;
    private View navHome, navAI, navProfile;
    private MaterialButton btnAskAI;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_search_results);

        View mainView = findViewById(R.id.bottomNavigation).getParent() instanceof View ? (View)findViewById(R.id.bottomNavigation).getParent() : findViewById(android.R.id.content);
        ViewCompat.setOnApplyWindowInsetsListener(mainView, (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        initViews();
        setupRecyclerView();
        setupNavigation();
        handleIntentData();
    }

    private void initViews() {
        tvSearchQuery = findViewById(R.id.tvSearchQuery);
        tvNoResults = findViewById(R.id.tvNoResults);
        progressBar = findViewById(R.id.progressBar);
        rvMedicines = findViewById(R.id.rvMedicines);
        btnAskAI = findViewById(R.id.btnAskAI);
        
        navHome = findViewById(R.id.navHome);
        navAI = findViewById(R.id.navAI);
        navProfile = findViewById(R.id.navProfile);

        findViewById(R.id.btnMenu).setOnClickListener(v -> finish());
    }

    private void setupRecyclerView() {
        adapter = new MedicineAdapter(medicineList, medicine -> {
            Intent intent = new Intent(this, MedicineDetailActivity.class);
            intent.putExtra("MEDICINE_ID", medicine.getMedicineId());
            startActivity(intent);
        });
        rvMedicines.setLayoutManager(new LinearLayoutManager(this));
        rvMedicines.setAdapter(adapter);
    }

    private void setupNavigation() {
        if (navHome != null) navHome.setOnClickListener(v -> {
            Intent intent = new Intent(this, HomeActivity.class);
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
            startActivity(intent);
            finish();
        });

        if (navAI != null) navAI.setOnClickListener(v -> {
            startActivity(new Intent(this, ChatAIActivity.class));
            finish();
        });

        if (btnAskAI != null) btnAskAI.setOnClickListener(v -> {
            Intent intent = new Intent(this, ChatAIActivity.class);
            intent.putExtra("INITIAL_QUERY", query);
            startActivity(intent);
        });
    }

    private void handleIntentData() {
        query = getIntent().getStringExtra("SYMPTOMS_QUERY");
        if (query != null && !query.isEmpty()) {
            tvSearchQuery.setText(getString(R.string.based_on_symptoms, query));
            searchMedicinesFromDb(query);
        }
    }

    private void searchMedicinesFromDb(String keyword) {
        progressBar.setVisibility(View.VISIBLE);
        tvNoResults.setVisibility(View.GONE);
        
        RetrofitClient.getApiService().searchMedicines(keyword).enqueue(new Callback<List<MedicineDto>>() {
            @Override
            public void onResponse(@NonNull Call<List<MedicineDto>> call, @NonNull Response<List<MedicineDto>> response) {
                progressBar.setVisibility(View.GONE);
                if (response.isSuccessful() && response.body() != null) {
                    List<MedicineDto> results = response.body();
                    medicineList.clear();
                    if (results.isEmpty()) {
                        tvNoResults.setVisibility(View.VISIBLE);
                    } else {
                        medicineList.addAll(results);
                        rvMedicines.scrollToPosition(0); // Cuộn lên đầu khi có kết quả mới
                    }
                    adapter.notifyDataSetChanged();
                }
            }

            @Override
            public void onFailure(@NonNull Call<List<MedicineDto>> call, @NonNull Throwable t) {
                progressBar.setVisibility(View.GONE);
                tvNoResults.setVisibility(View.VISIBLE);
                tvNoResults.setText("Lỗi kết nối: " + t.getMessage());
            }
        });
    }
}
