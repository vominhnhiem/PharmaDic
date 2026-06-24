using System.Net.Http.Headers;
using System.Text.Json;

namespace PharmaDicBackEnd.ApiService.Services;

public class GroqAiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GroqAiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["GroqApiKey"] ?? Environment.GetEnvironmentVariable("GROQ_API_KEY") ?? "";
    }

    public async Task<string> GenerateContentAsync(string systemPrompt, string userPrompt)
    {
        if (string.IsNullOrEmpty(_apiKey)) throw new Exception("Groq API Key is not configured.");

        string url = "https://api.groq.com/openai/v1/chat/completions";

        var requestBody = new
        {
            model = "llama-3.1-8b-instant", // Sử dụng Llama 3.1 8B của Groq
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            },
            temperature = 0.5
        };

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        requestMessage.Content = JsonContent.Create(requestBody);

        var response = await _httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            throw new Exception($"Groq API error ({response.StatusCode}): {err}");
        }

        var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

        // Phân tích JSON trả về của chuẩn OpenAI: choices[0].message.content
        var content = jsonResponse
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return content ?? string.Empty;
    }
}
