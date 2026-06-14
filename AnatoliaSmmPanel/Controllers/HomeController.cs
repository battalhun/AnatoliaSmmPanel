using AnatoliaSmmPanel.Areas.Admin.Services;
using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Data.Models.Admin;
using AnatoliaSmmPanel.Data.Models.Appliciton;
using AnatoliaSmmPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnatoliaSmmPanel.Areas.Admin.Dtos;
using AnatoliaSmmPanel.ViewModels;

namespace AnatoliaSmmPanel.Controllers
{   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISmmApiService _smmApiService;
        private readonly ISettingsService _settingsService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, ISmmApiService smmApiService, ISettingsService settingsService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _smmApiService = smmApiService;
            _settingsService = settingsService;
        }

        // ── New Order Page ──────────────────────────────────────
        [Authorize]
        [HttpGet("new-order")]
        public IActionResult NewOrder()
        {
            var userId = _userManager.GetUserId(User);

            var user = _userManager.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.Balance, u.Spent })
                .FirstOrDefault();

            ViewData["Balance"] = user?.Balance ?? 0m;
            ViewData["Spent"] = user?.Spent ?? 0m;

            return View();
        }

        // ── GetServices (AJAX) ─────────────────────────────────
        [Authorize]
        [HttpGet("home/GetServices")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetServices()
        {
            var categories = await _context.ServiceCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder)
                .Select(c => new { id = c.Id, name = c.Name })
                .ToListAsync();

            var services = await _context.SmmServices
                .Where(s => s.IsActive)
                .OrderBy(s => s.SortOrder)
                .Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    categoryId = s.serviceCategoryId,
                    rate = s.Rate,
                    minOrder = s.Min,
                    maxOrder = s.Max,
                    //description = s.Description,
                    type = s.Type,
                    dripfeed = s.Dripfeed,
                    refill = s.Refill,
                    cancel = s.Cancel,
                })
                .ToListAsync();

            return Json(new { success = true, categories, services });
        }

        // ── PlaceOrder (AJAX POST) ──────────────────────────────
        [Authorize]
        [HttpPost("home/PlaceOrder")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            // Servisi kontrol et
            var service = await _context.SmmServices
                .FirstOrDefaultAsync(s => s.Id == request.ServiceId && s.IsActive);

            if (service == null)
                return Json(new { success = false, message = "Service not found or inactive." });

            // Quantity kontrolü
            if (request.Quantity < service.Min || request.Quantity > service.Max)
                return Json(new { success = false, message = $"Quantity must be between {service.Min} and {service.Max}." });

            // Kullanıcıyı al
            var guidId = _userManager.GetUserId(User);        // GUID string — FindByIdAsync için
            var intUserId = _userManager.Users
                .Where(u => u.Id == guidId)
                .Select(u => u.UserId)
                .FirstOrDefault();                               // int UserId — Order.UserId için

            var user = await _userManager.FindByIdAsync(guidId!); // Identity işlemleri için GUID

            if (user == null)
                return Json(new { success = false, message = "User not found." });

            // Charge hesapla
            var charge = (request.Quantity * service.Rate) / 1000m;

            // Bakiye kontrolü
            if (user.Balance < charge)
                return Json(new { success = false, message = $"Insufficient balance. Required: ${charge:0.0000}, Available: ${user.Balance:0.0000}" });

            // Order oluştur
            var order = new Order
            {
                UserId = intUserId!,
              //  Nickname = user.Nickname ?? user.Email,
                ServiceId = service.Id,
                Link = request.Link,
                Quantity = request.Quantity,
                StartCount = 0,
                Remains = request.Quantity,
                Charge = charge,
                Status = "Pending",
                Mode = "Auto",
                CreatedAt = DateTime.UtcNow,
            };

            // Bakiyeyi düş
            user.Balance -= charge;
            user.Spent += charge;

            _context.Orders.Add(order);
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            int orderId = order.Id; 
            var newOrder = await _context.SmmServices
              .Include(s => s.Provider)
              .Include(s => s.externalServiceInfo)
              .Where(s => s.Id == service.Id)
              .Select(s => new AddOrderResponse
              {
                  apiurl = s.Provider.ApiUrl,
                  apikey = s.Provider.ApiKey,
                  serviceId = s.externalServiceInfo.ExternalServiceId,
                  link = request.Link,
                  quantity = request.Quantity
              })
              .FirstOrDefaultAsync();

            SmmOrderResponseDto apiResponse = await _smmApiService.AddOrderAsync(
     newOrder.apiurl,
     newOrder.apikey,
     newOrder.serviceId,
     newOrder.link,
     newOrder.quantity,
     HttpMethod.Post
 );

            // Gelen cevabı kontrol ediyoruz
            if (apiResponse != null && string.IsNullOrEmpty(apiResponse.Error))
            {
      
                var externalOrderId = apiResponse.OrderId;

               

                _context.Orders.Where(o => o.Id == orderId).ExecuteUpdate(s => s
                    .SetProperty(o => o.ExternalOrderId, externalOrderId.ToString())
                    .SetProperty(o => o.Status, "Processing")
                    .SetProperty(o => o.UpdatedAt, DateTime.UtcNow)
                );

                _context.SaveChanges();
            }
            else
            {
                // Hata varsa kullanıcıya göster
                return Json(new { success = false, message = apiResponse?.Error ?? "Bilinmeyen bir API hatası oluştu." });
            }




            return Json(new
            {
                success = true,
                orderId = order.Id,
                charge = charge.ToString("0.0000"),
                balance = user.Balance.ToString("0.0000")
            });
        }

        // ── Other Pages ─────────────────────────────────────────
        [Authorize]
        [HttpGet("services")]
        public IActionResult Services()
        {
            List<HomeServicesViewModel> services = _context.SmmServices
                .Include(s => s.serviceCategory)
                .Where(s => s.IsActive)
                .OrderBy(s => s.SortOrder)
                .Select(s => new HomeServicesViewModel
                {
                    Id = s.Id,
                    CategoryId = s.serviceCategoryId ?? 0,
                    CategoryName = s.serviceCategory != null ? s.serviceCategory.Name : "General",
                    Name = s.Name,
                    Rate = s.Rate,
                    Min = s.Min,
                    Max = s.Max,
                    Type = s.Type,
                //    Description = s.Description,
                    Dripfeed = s.Dripfeed,
                    Refill = s.Refill,
                    Cancel = s.Cancel
                })
                .ToList();

            return View(services);
        }

        [Authorize]
        [HttpGet("tickets")]
        public IActionResult Tickets() => View();
    }

    // ── DTO ────────────────────────────────────────────────────
    public class PlaceOrderRequest
    {
        public int ServiceId { get; set; }
        public string Link { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int Runs { get; set; }
        public int Interval { get; set; }
    }

    public class AddOrderResponse
    {
        public string apiurl { get; set; }
        public string apikey { get; set; }
        public string serviceId { get; set; }
        public string link { get; set; }
        public int quantity { get; set; }
    }
}
