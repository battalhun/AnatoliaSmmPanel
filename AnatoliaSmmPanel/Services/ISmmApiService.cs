using AnatoliaSmmPanel.Areas.Admin.Dtos;

namespace AnatoliaSmmPanel.Services
{
    public interface ISmmApiService
    {
        // HttpMethod parametresi opsiyonel (default: POST)
        Task<List<SmmServiceDto>> GetServicesAsync(string apiUrl, string apiKey, HttpMethod? method = null);
        Task<SmmBalanceDto> GetBalanceAsync(string apiUrl, string apiKey, HttpMethod? method = null);
        Task<SmmOrderResponseDto> AddOrderAsync(string apiUrl, string apiKey, string serviceId, string link, int quantity, HttpMethod? method = null);
        Task<SmmOrderStatusDto> GetOrderStatusAsync(string apiUrl, string apiKey, int orderId, HttpMethod? method = null);

        // Provider yanıtı bir ARRAY döner, dönüş tipi List olmalı.
        Task<List<SmmCancelOrderResponseDto>> AddCancelOrderAsync(string apiUrl, string apiKey, IEnumerable<string> externalOrderIds, HttpMethod? method = null);

        Task<bool> IsValidSmmApiAsync(string apiUrl);
    }
}