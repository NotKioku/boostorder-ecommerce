using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using boostorder_ecommerce.Models;

namespace boostorder_ecommerce.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;
        private const string Username = "ck_b9e4e281dc7aa5595062207a479090a390304335";
        private const string Password = "cs_95b5c4724a48737ed72daf8314dae9cbc83842ae";
        private const string BaseUrl = "https://cloud.boostorder.com/bo-mart/api/v1/wp-json/wc/v1/bo/products";

        public string? LastError { get; private set; }

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetProductsAsync(int page = 1)
        {
            LastError = null;
            try
            {
                var requestUrl = $"{BaseUrl}?page={page}";
                using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                var authByteArray = Encoding.ASCII.GetBytes($"{Username}:{Password}");
                var authHeaderValue = Convert.ToBase64String(authByteArray);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    LastError = $"HTTP Error {(int)response.StatusCode}: {response.ReasonPhrase}";
                    Console.WriteLine($"[ProductApiService] {LastError}");
                    return new List<Product>();
                }

                var json = await response.Content.ReadAsStringAsync();

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                };

                List<Product> products = new();
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    var wrapper = JsonSerializer.Deserialize<ProductResponseWrapper>(json, jsonOptions);
                    products = wrapper?.Products ?? new List<Product>();
                }
                else if (doc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    products = JsonSerializer.Deserialize<List<Product>>(json, jsonOptions) ?? new List<Product>();
                }

                // Filter to return only products where type is "variable"
                var variableProducts = products
                    .Where(p => string.Equals(p.Type, "variable", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return variableProducts;
            }
            catch (Exception ex)
            {
                LastError = $"Exception: {ex.Message}";
                Console.WriteLine($"[ProductApiService] Error fetching products: {ex}");
                return new List<Product>();
            }
        }
    }
}
