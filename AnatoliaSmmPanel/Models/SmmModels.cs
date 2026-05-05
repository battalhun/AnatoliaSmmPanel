using System.Text.Json.Serialization;

namespace AnatoliaSmmPanel.Models
{
    // 1. Servis Listesi İçin
    public class SmmServiceDto
    {
        [JsonPropertyName("service")]
        public string Service { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("rate")]
        public string Rate { get; set; }

        [JsonPropertyName("min")]
        public string Min { get; set; }

        [JsonPropertyName("max")]
        public string Max { get; set; }

        [JsonPropertyName("refill")]
        public bool Refill { get; set; }
    }

    // 2. Bakiye Sorgusu İçin
    public class SmmBalanceDto
    {
        [JsonPropertyName("balance")]
        public string Balance { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; } // Hata varsa buraya gelir
    }

    // 3. Sipariş Oluşturma (Add Order) Sonucu İçin
    public class SmmOrderResponseDto
    {
        [JsonPropertyName("order")]
        public int? OrderId { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }

    // 4. Sipariş Durumu (Status) Sonucu İçin
    public class SmmOrderStatusDto
    {
        [JsonPropertyName("charge")]
        public string Charge { get; set; }

        [JsonPropertyName("start_count")]
        public string StartCount { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("remains")]
        public string Remains { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}