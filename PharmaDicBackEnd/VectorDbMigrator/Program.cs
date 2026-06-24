using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace VectorDbMigrator
{
    class Program
    {
        // 1. CẤU HÌNH KẾT NỐI
        const string SqlConnectionString = "Server=KHOI\\SQLEXPRESS;Database=PharmacyApp;Trusted_Connection=True;TrustServerCertificate=True;";
        // Lấy API Key từ biến môi trường, ném lỗi nếu chưa được cấu hình
        static readonly string GoogleApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")
            ?? throw new Exception("Thiếu GOOGLE_API_KEY trong Environment Variables!");

        // Cổng mặc định khi chạy qdrant.exe
        const string CollectionName = "MedicinesCollection";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Bắt đầu quá trình đồng bộ SQL -> Vector DB...");

            // 2. KHỞI TẠO QDRANT COLLECTION
            var qdrantClient = new QdrantClient("localhost", 6334);
            var collectionExists = await qdrantClient.CollectionExistsAsync(CollectionName);

            if (!collectionExists)
            {
                Console.WriteLine($"Đang tạo Collection mới: {CollectionName} (768 dimensions)...");
                // Quan trọng: Kích thước vector của Google Gemini là 768
                await qdrantClient.CreateCollectionAsync(
                    collectionName: CollectionName,
                    vectorsConfig: new VectorParams { Size = 768, Distance = Distance.Cosine }
                );
            }
            else
            {
                Console.WriteLine($"Collection {CollectionName} đã tồn tại, tiếp tục nạp dữ liệu...");
            }

            // 3. ĐỌC DỮ LIỆU TỪ SQL SERVER
            Console.WriteLine("Đang lấy dữ liệu từ SQL Server...");
            List<MedicineDto> medicines;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                // Lấy thông tin thuốc kèm nhóm thuốc
                string sql = @"
                    SELECT 
                        m.MedicineID, 
                        m.MedicineName, 
                        m.Uses, 
                        mc.CategoryName
                    FROM Medicines m
                    LEFT JOIN MedicineCategories mc ON m.CategoryID = mc.CategoryID";

                medicines = connection.Query<MedicineDto>(sql).ToList();
            }
            Console.WriteLine($"Tìm thấy {medicines.Count} loại thuốc trong PharmacyApp.");

            // 4. CHUYỂN ĐỔI VECTOR VÀ LƯU VÀO QDRANT
            using var httpClient = new HttpClient();
            var points = new List<PointStruct>();

            foreach (var med in medicines)
            {
                // Gom dữ liệu thành một câu văn để AI dễ "hiểu" ngữ nghĩa
                string textToEmbed = $"Tên thuốc: {med.MedicineName}. Nhóm: {med.CategoryName}. Công dụng điều trị: {med.Uses}.";

                Console.WriteLine($"Đang phân tích ngữ nghĩa (embedding): {med.MedicineName}...");

                try
                {
                    // Lấy Vector 768 chiều từ Google AI
                    float[] vector = await GetGoogleEmbeddingAsync(httpClient, textToEmbed);

                    // Tạo bản ghi cho Vector DB
                    var point = new PointStruct
                    {
                        Id = (ulong)med.MedicineID, // Bắt buộc ID phải khớp với SQL Server
                        Vectors = vector,
                        Payload =
                        {
                            ["MedicineName"] = med.MedicineName,
                            ["Category"] = med.CategoryName ?? ""
                        }
                    };
                    points.Add(point);

                    // Delay 0.5s để Google không đánh dấu là Spam request
                    await Task.Delay(500);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi xử lý {med.MedicineName}: {ex.Message}");
                }
            }

            // 5. ĐẨY LÊN QDRANT SERVER
            if (points.Any())
            {
                Console.WriteLine("Đang lưu dữ liệu vào Qdrant Server...");
                await qdrantClient.UpsertAsync(CollectionName, points);
                Console.WriteLine("✅ ĐỒNG BỘ THÀNH CÔNG TẤT CẢ DỮ LIỆU!");
            }
        }

        // HÀM GIAO TIẾP VỚI GOOGLE AI ĐỂ LẤY VECTOR
        static async Task<float[]> GetGoogleEmbeddingAsync(HttpClient client, string text)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-embedding-001:embedContent?key={GoogleApiKey}";
            var requestBody = new
            {
                model = "models/gemini-embedding-001",
                content = new { parts = new[] { new { text = text } } }
            };

            var response = await client.PostAsJsonAsync(url, requestBody);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
            return jsonResponse
                .GetProperty("embedding")
                .GetProperty("values")
                .EnumerateArray()
                .Select(x => x.GetSingle())
                .ToArray();
        }
    }

    // Class hứng dữ liệu từ Dapper
    public class MedicineDto
    {
        public int MedicineID { get; set; }
        public string MedicineName { get; set; }
        public string Uses { get; set; }
        public string CategoryName { get; set; }
    }
}