package com.example.pharmadicmobile.api;

import com.example.pharmadicmobile.dtos.*;
import java.util.List;
import retrofit2.Call;
import retrofit2.http.*;

public interface ApiService {
    
    // --- AUTH ---
    @POST("api/Auth/login")
    Call<TokenResponse> login(@Body LoginRequest request);

    @POST("api/Auth/register")
    Call<Void> register(@Body UserCreateDto dto);
    
    // --- USER ---
    @GET("api/User")
    Call<List<UserResponseDto>> getAllUsers(@Header("Authorization") String token);

    // --- CHAT AI ---
    @POST("api/Ai/chat")
    Call<AiResponse> askAi(@Header("Authorization") String token, @Body ChatRequest request);

    @GET("api/Ai/history/{userId}")
    Call<List<Object>> getChatHistory(@Header("Authorization") String token, @Path("userId") int userId);

    // --- MEDICINE & DISEASE ---
    @GET("api/Medicine/search")
    Call<List<MedicineDto>> searchMedicines(@Query("keyword") String keyword);

    @GET("api/Medicine/{id}")
    Call<MedicineDetailDto> getMedicineDetail(@Path("id") int id);

    @GET("api/Disease/search")
    Call<List<DiseaseSummaryDto>> searchDiseases(@Query("keyword") String keyword);

    // --- HISTORY ---
    // Giả sử backend có endpoint này, nếu chưa có bạn có thể dùng tạm history chat làm "Báo cáo"
    @GET("api/User/search-history/{userId}")
    Call<List<Object>> getUserSearchHistory(@Header("Authorization") String token, @Path("userId") int userId);
}
