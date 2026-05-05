using AnatoliaSmmPanel.Models;
using System.Text.Json;
using System.Web;

namespace AnatoliaSmmPanel.Services
{
    public class SmmApiService : ISmmApiService
    {
        private readonly HttpClient _httpClient;

        public SmmApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // --- ANA İSTEK MOTORU (HEM GET HEM POST DESTEKLER) ---
        private async Task<string> SendRequestAsync(string url, Dictionary<string, string> parameters, HttpMethod method)
        {
            HttpResponseMessage response;

            if (method == HttpMethod.Get)
            {
                // GET İşlemi: Parametreleri URL'ye ?key=...&action=... şeklinde ekle
                var queryParams = string.Join("&", parameters.Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}"));
                var fullUrl = $"{url}?{queryParams}";

                response = await _httpClient.GetAsync(fullUrl);
            }
            else
            {
                // POST İşlemi: Parametreleri FormUrlEncoded olarak Body'de gönder (Varsayılan)
                var content = new FormUrlEncodedContent(parameters);
                response = await _httpClient.PostAsync(url, content);
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // 1. Servisleri Listele (action=services)
        public async Task<List<SmmServiceDto>> GetServicesAsync(string apiUrl, string apiKey, HttpMethod? method = null)
        {
            var parameters = new Dictionary<string, string>
            {
                { "key", apiKey },
                { "action", "services" }
            };

            var jsonResponse = await SendRequestAsync(apiUrl, parameters, method ?? HttpMethod.Post);
            return JsonSerializer.Deserialize<List<SmmServiceDto>>(jsonResponse) ?? new List<SmmServiceDto>();
        }

        // 2. Bakiye Sorgula (action=balance)
        public async Task<SmmBalanceDto> GetBalanceAsync(string apiUrl, string apiKey, HttpMethod? method = null)
        {
            var parameters = new Dictionary<string, string>
            {
                { "key", apiKey },
                { "action", "balance" }
            };

            var jsonResponse = await SendRequestAsync(apiUrl, parameters, method ?? HttpMethod.Post);
            return JsonSerializer.Deserialize<SmmBalanceDto>(jsonResponse) ?? new SmmBalanceDto();
        }

        // 3. Sipariş Ver (action=add)
        public async Task<SmmOrderResponseDto> AddOrderAsync(string apiUrl, string apiKey, string serviceId, string link, int quantity, HttpMethod? method = null)
        {
            var parameters = new Dictionary<string, string>
            {
                { "key", apiKey },
                { "action", "add" },
                { "service", serviceId },
                { "link", link },
                { "quantity", quantity.ToString() }
            };

            var jsonResponse = await SendRequestAsync(apiUrl, parameters, method ?? HttpMethod.Post);
            return JsonSerializer.Deserialize<SmmOrderResponseDto>(jsonResponse) ?? new SmmOrderResponseDto();
        }

        // 4. Sipariş Durumu Sorgula (action=status)
        public async Task<SmmOrderStatusDto> GetOrderStatusAsync(string apiUrl, string apiKey, int orderId, HttpMethod? method = null)
        {
            var parameters = new Dictionary<string, string>
            {
                { "key", apiKey },
                { "action", "status" },
                { "order", orderId.ToString() }
            };

            var jsonResponse = await SendRequestAsync(apiUrl, parameters, method ?? HttpMethod.Post);
            return JsonSerializer.Deserialize<SmmOrderStatusDto>(jsonResponse) ?? new SmmOrderStatusDto();
        }

        // Sistemin SMM paneli olup olmadığını test eden metot
        public async Task<bool> IsValidSmmApiAsync(string apiUrl)
        {
            try
            {
                // Sahte bir istek atıyoruz
                var parameters = new Dictionary<string, string>
        {
            { "key", "test_ping_123" },
            { "action", "balance" } // Sadece panellerin anlayacağı bir parametre
        };

                var content = new FormUrlEncodedContent(parameters);

                // 5 saniye içinde cevap gelmezse patlamaması için (Opsiyonel ama önerilir)
                _httpClient.Timeout = TimeSpan.FromSeconds(10);

                var response = await _httpClient.PostAsync(apiUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                // Eğer dönen cevap geçerli bir JSON ise, burası %99 ihtimalle bir API'dir.
                // Hata dönse bile (Yanlış key vb.) JSON olduğu için SMM paneli olduğunu anlarız.
                using (JsonDocument doc = JsonDocument.Parse(responseString))
                {
                    return true;
                }
            }
            catch
            {
                // URL'ye ulaşılamazsa, 404 dönerse veya HTML sayfası (JSON olmayan bir şey) dönerse buraya düşer
                return false;
            }
        }
    }
}
