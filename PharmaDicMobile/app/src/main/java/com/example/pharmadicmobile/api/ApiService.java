package com.example.pharmadicmobile.api;

import com.example.pharmadicmobile.dtos.*;
import java.util.List;
import retrofit2.Call;
import retrofit2.http.*;

public interface ApiService {
    
    // --- AUTH ---
    @POST("api/Auth/login")
    Call<TokenResponse> login(@Body LoginRequest request);

    // Backend không có register trong AuthController nhưng có trong UserController
    // Nếu bạn muốn dùng register public, hãy đảm bảo Backend có endpoint này.
    
    // --- USER (ADMIN ONLY) ---
    @GET("api/User")
    Call<List<UserResponseDto>> getAllUsers(@Header("Authorization") String token);

    @POST("api/User")
    Call<Void> createUser(@Header("Authorization") String token, @Body UserCreateDto dto);

    @PUT("api/User/{id}")
    Call<Void> updateUser(@Header("Authorization") String token, @Path("id") int id, @Body UserUpdateDto dto);

    @DELETE("api/User/{id}")
    Call<Void> deleteUser(@Header("Authorization") String token, @Path("id") int id);

    // --- CHAT AI ---
    @POST("api/Ai/chat")
    Call<AiResponse> askAi(@Header("Authorization") String token, @Body ChatRequest request);

    @POST("api/Ai/check-interaction")
    Call<AiResponse> checkInteraction(@Header("Authorization") String token, @Body InteractionRequest request);

    @POST("api/Ai/suggest-regimen")
    Call<AiResponse> suggestRegimen(@Header("Authorization") String token, @Body RegimenRequest request);

    // --- MEDICINE ---
    @GET("api/Medicine/search")
    Call<List<MedicineDto>> searchMedicines(@Query("keyword") String keyword);

    @GET("api/Medicine/{id}")
    Call<MedicineDetailDto> getMedicineDetail(@Path("id") int id);

    // --- DISEASE ---
    @GET("api/Disease/search")
    Call<List<DiseaseSummaryDto>> searchDiseases(@Query("keyword") String keyword);

    @GET("api/Disease/{id}")
    Call<DiseaseDetailDto> getDiseaseDetail(@Path("id") int id);
}
