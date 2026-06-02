using System.Text.Json.Serialization;

namespace AnatoliaSmmPanel.Areas.Admin.Dtos
{
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
