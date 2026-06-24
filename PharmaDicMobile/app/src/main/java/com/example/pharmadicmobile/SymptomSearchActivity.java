package com.example.pharmadicmobile;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.Toast;
import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import com.google.android.material.chip.Chip;
import com.google.android.material.chip.ChipGroup;

public class SymptomSearchActivity extends AppCompatActivity {

    private Button btnSearchSymptoms;
    private EditText etSymptoms;
    private ChipGroup chipGroupSuggestions;
    private LinearLayout navHome, navAI, navHistory;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_symptom_search);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        initViews();
        setupClickListeners();
        setupChipListeners();
    }

    private void initViews() {
        btnSearchSymptoms = findViewById(R.id.btnSearchSymptoms);
        etSymptoms = findViewById(R.id.etSymptoms);
        chipGroupSuggestions = findViewById(R.id.chipGroupSuggestions);
        
        navHome = findViewById(R.id.navHome);
        navAI = findViewById(R.id.navAI);
        navHistory = findViewById(R.id.navHistory);
    }

    private void setupClickListeners() {
        btnSearchSymptoms.setOnClickListener(v -> {
            String symptoms = etSymptoms.getText().toString().trim();
            if (symptoms.isEmpty()) {
                Toast.makeText(this, getString(R.string.empty_symptoms), Toast.LENGTH_SHORT).show();
                return;
            }

            Intent intent = new Intent(SymptomSearchActivity.this, SearchResultsActivity.class);
            intent.putExtra("SYMPTOMS_QUERY", symptoms);
            startActivity(intent);
        });

        if (navHome != null) navHome.setOnClickListener(v -> finish());
        
        if (navAI != null) navAI.setOnClickListener(v -> {
            startActivity(new Intent(this, ChatAIActivity.class));
            finish();
        });
        
        if (navHistory != null) navHistory.setOnClickListener(v -> {
            startActivity(new Intent(this, HistoryActivity.class));
        });
    }

    private void setupChipListeners() {
        if (chipGroupSuggestions == null) return;

        for (int i = 0; i < chipGroupSuggestions.getChildCount(); i++) {
            View child = chipGroupSuggestions.getChildAt(i);
            if (child instanceof Chip) {
                Chip chip = (Chip) child;
                chip.setOnClickListener(v -> {
                    String currentText = etSymptoms.getText().toString();
                    String chipText = chip.getText().toString();
                    
                    if (currentText.isEmpty()) {
                        etSymptoms.setText(chipText);
                    } else {
                        etSymptoms.setText(getString(android.R.string.copy, currentText + ", " + chipText).replace("Copy", "")); 
                        // Hoặc dùng StringBuilder/String.format cho chuẩn
                        etSymptoms.setText(currentText + ", " + chipText);
                    }
                    etSymptoms.setSelection(etSymptoms.getText().length());
                });
            }
        }
    }
}
