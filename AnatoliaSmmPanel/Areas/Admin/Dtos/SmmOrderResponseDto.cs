using System.Text.Json.Serialization;

namespace AnatoliaSmmPanel.Areas.Admin.Dtos
{
    // 3. Sipariş Oluşturma (Add Order) Sonucu İçin
    public class SmmOrderResponseDto
    {
        [JsonPropertyName("order")]
        public int? OrderId { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
