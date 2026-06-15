using AnatoliaSmmPanel.Areas.Admin.Services;
using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Data.Models.Appliciton;
using AnatoliaSmmPanel.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnatoliaSmmPanel.Controllers
{
    // ── HomeController.cs ─────────────────────────────────────────
    // Ana dosya: alanlar, constructor, ortak helper.
    // Diğer partial dosyalar: HomeController.NewOrder.cs,
    // HomeController.Services.cs, HomeController.Tickets.cs,
    // HomeController.Orders.cs
    public partial class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISmmApiService _smmApiService;
        private readonly ISettingsService _settingsService;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ISmmApiService smmApiService,
            ISettingsService settingsService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _smmApiService = smmApiService;
            _settingsService = settingsService;
        }

        // ── Ortak helper: GUID (Identity Id) -> int UserId lookup ────
        // Order/Ticket gibi modüllerde FK olarak kullanılan int UserId,
        // tüm partial dosyalarda bu metod üzerinden alınır.
        protected int GetIntUserId()
        {
            var guidId = _userManager.GetUserId(User);
            return _userManager.Users
                .Where(u => u.Id == guidId)
                .Select(u => u.UserId)
                .FirstOrDefault();
        }

        // ── Ortak helper: Identity GUID Id ───────────────────────────
        protected string? GetGuidUserId() => _userManager.GetUserId(User);
    }
}
