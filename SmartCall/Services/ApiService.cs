using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartCall.Services
{
    public class ApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string _baseUrl = "http://localhost:5000/api"; // URL do backend
        private static string? _token = null;

        static ApiService()
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public static void SetBaseUrl(string url)
        {
            _baseUrl = url.TrimEnd('/') + "/api";
        }

        public static void SetToken(string token)
        {
            _token = token;
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }

        public static void ClearToken()
        {
            _token = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public static string? GetToken()
        {
            return _token;
        }

        public static bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(_token);
        }

        private static async Task<T?> SendRequestAsync<T>(
            HttpMethod method, 
            string endpoint, 
            object? body = null)
        {
            try
            {
                var request = new HttpRequestMessage(method, $"{_baseUrl}/{endpoint}");

                if (body != null)
                {
                    var json = JsonSerializer.Serialize(body);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException(
                        $"Erro na requisição: {response.StatusCode} - {errorContent}");
                }

                var content = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrWhiteSpace(content))
                {
                    return default(T);
                }

                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao comunicar com o servidor: {ex.Message}", ex);
            }
        }

        public static async Task<T?> GetAsync<T>(string endpoint)
        {
            return await SendRequestAsync<T>(HttpMethod.Get, endpoint);
        }

        public static async Task<T?> PostAsync<T>(string endpoint, object body)
        {
            return await SendRequestAsync<T>(HttpMethod.Post, endpoint, body);
        }

        public static async Task<T?> PutAsync<T>(string endpoint, object body)
        {
            return await SendRequestAsync<T>(HttpMethod.Put, endpoint, body);
        }

        public static async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                await SendRequestAsync<object>(HttpMethod.Delete, endpoint);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
