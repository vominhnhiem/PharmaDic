package com.example.pharmadicmobile;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.Toast;
import androidx.activity.EdgeToEdge;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.pharmadicmobile.adapters.ChatAdapter;
import com.example.pharmadicmobile.api.RetrofitClient;
import com.example.pharmadicmobile.dtos.AiResponse;
import com.example.pharmadicmobile.dtos.ChatRequest;
import com.example.pharmadicmobile.models.ChatMessage;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ChatAIActivity extends AppCompatActivity {

    private LinearLayout navHome, navSearch, navProfile, navAI;
    private ImageView btnBack, btnSend;
    private EditText edtMessage;
    private RecyclerView chatRecyclerView;
    private ChatAdapter chatAdapter;
    private List<ChatMessage> chatMessages;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_chat_ai);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(android.R.id.content), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        initViews();
        setupRecyclerView();
        setupClickListeners();

        addMessage("Chào Dược sĩ, tôi có thể hỗ trợ gì cho bạn trong việc tra cứu tương tác thuốc hoặc liều dùng lâm sàng hôm nay?", false);
    }

    private void initViews() {
        btnBack = findViewById(R.id.btnBack);
        navHome = findViewById(R.id.navHome);
        navSearch = findViewById(R.id.navSearch);
        navAI = findViewById(R.id.navAI);
        navProfile = findViewById(R.id.navProfile);
        btnSend = findViewById(R.id.btnSend);
        edtMessage = findViewById(R.id.edtMessage);
        chatRecyclerView = findViewById(R.id.chatRecyclerView);
    }

    private void setupRecyclerView() {
        chatMessages = new ArrayList<>();
        chatAdapter = new ChatAdapter(chatMessages);
        chatRecyclerView.setLayoutManager(new LinearLayoutManager(this));
        chatRecyclerView.setAdapter(chatAdapter);
    }

    private void setupClickListeners() {
        if (btnBack != null) btnBack.setOnClickListener(v -> finish());

        if (navHome != null) {
            navHome.setOnClickListener(v -> {
                Intent intent = new Intent(this, HomeActivity.class);
                intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
                startActivity(intent);
                finish();
            });
        }

        if (navSearch != null) {
            navSearch.setOnClickListener(v -> {
                startActivity(new Intent(this, SymptomSearchActivity.class));
                finish();
            });
        }

        if (navProfile != null) {
            navProfile.setOnClickListener(v -> {
                startActivity(new Intent(this, ProfileActivity.class));
                finish();
            });
        }

        if (btnSend != null) {
            btnSend.setOnClickListener(v -> {
                String question = edtMessage.getText().toString().trim();
                if (!question.isEmpty()) {
                    sendMessageToAI(question);
                }
            });
        }
    }

    private void sendMessageToAI(String question) {
        addMessage(question, true);
        edtMessage.setText("");

        SharedPreferences sharedPreferences = getSharedPreferences("PharmaDic", Context.MODE_PRIVATE);
        String token = sharedPreferences.getString("token", "");
        
        if (token.isEmpty()) {
            addMessage("Bạn cần đăng nhập để sử dụng tính năng này.", false);
            return;
        }

        String authHeader = "Bearer " + token;

        ChatRequest request = new ChatRequest(question);
        RetrofitClient.getApiService().askAi(authHeader, request).enqueue(new Callback<AiResponse>() {
            @Override
            public void onResponse(@NonNull Call<AiResponse> call, @NonNull Response<AiResponse> response) {
                if (response.isSuccessful() && response.body() != null) {
                    addMessage(response.body().getAnswer(), false);
                } else {
                    addMessage("Xin lỗi, tôi không thể xử lý câu hỏi lúc này.", false);
                }
            }

            @Override
            public void onFailure(@NonNull Call<AiResponse> call, @NonNull Throwable t) {
                addMessage("Lỗi kết nối mạng: " + t.getMessage(), false);
            }
        });
    }

    private void addMessage(String content, boolean isUser) {
        String currentTime = new SimpleDateFormat("hh:mm a", Locale.getDefault()).format(new Date());
        chatMessages.add(new ChatMessage(content, isUser, currentTime));
        chatAdapter.notifyItemInserted(chatMessages.size() - 1);
        chatRecyclerView.scrollToPosition(chatMessages.size() - 1);
    }
}
