using AnatoliaSmmPanel.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnatoliaSmmPanel.Controllers
{
    public class SmmApiController : Controller
    {
        private readonly ISmmApiService _smmApiService;

        public SmmApiController(ISmmApiService smmApiService)
        {
            _smmApiService = smmApiService;
        }

        public async Task<IActionResult> TestApi()
        {
            string url = "https://tarantulasmm.com/api/v2";
            string key = "API_KEY_BURAYA";

            // 1. Örnek: GET metoduyla bakiye sorgulama (URL'den gider)
            var balance = await _smmApiService.GetBalanceAsync(url, key, HttpMethod.Get);

            // 2. Örnek: POST metoduyla sipariş verme (Body'den gider - Önerilen)
            var order = await _smmApiService.AddOrderAsync(url, key, "123", "https://instagram.com/test", 1000, HttpMethod.Post);

            // Veya method parametresini boş bırakın, otomatik POST kullanır:
            // var order = await _smmApiService.AddOrderAsync(url, key, "123", "https://instagram.com/test", 1000);

            return Ok(balance);
        }
    }
}
