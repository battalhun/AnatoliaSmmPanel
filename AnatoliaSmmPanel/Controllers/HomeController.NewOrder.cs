using AnatoliaSmmPanel.Areas.Admin.Dtos;
using AnatoliaSmmPanel.Data.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnatoliaSmmPanel.Controllers
{
    // HomeController.NewOrder.cs 
    public partial class HomeController
    {
        // New-order Sayfası
        [Authorize]
        [HttpGet("new-order")]
        public IActionResult NewOrder()
        {
            var userId = GetGuidUserId();

            var user = _userManager.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.Balance, u.Spent })
                .FirstOrDefault();

            ViewData["Balance"] = user?.Balance ?? 0m;
            ViewData["Spent"] = user?.Spent ?? 0m;

            return View();
        }

        // Ajax ile Servisleri ve Kategorileri new-order Sayfasına yükler
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

        // Yeni Sipariş oluştur
        [Authorize]
        [HttpPost("home/PlaceOrder")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            var service = await _context.SmmServices
                .Include(s => s.Provider)
                .FirstOrDefaultAsync(s => s.Id == request.ServiceId && s.IsActive);

            if (service == null)
                return Json(new { success = false, message = "Service not found or inactive." });

            if (request.Quantity < service.Min || request.Quantity > service.Max)
                return Json(new { success = false, message = $"Quantity must be between {service.Min} and {service.Max}." });

            var guidId = GetGuidUserId();
            var intUserId = _userManager.Users
                .Where(u => u.Id == guidId)
                .Select(u => u.UserId)
                .FirstOrDefault();

            var user = await _userManager.FindByIdAsync(guidId!);

            if (user == null)
                return Json(new { success = false, message = "User not found." });

            var charge = (request.Quantity * service.Rate) / 1000m;

            if (user.Balance < charge)
                return Json(new { success = false, message = $"Insufficient balance. Required: ${charge:0.0000}, Available: ${user.Balance:0.0000}" });

            var order = new Order
            {
                UserId = intUserId!,
                ServiceId = service.Id,
                Link = request.Link,
                Quantity = request.Quantity,
                StartCount = 0,
                Remains = request.Quantity,
                Charge = charge,
                Status = "Pending",
                Mode = "Auto",
                CreatedAt = DateTime.UtcNow,
                ProviderId = service.Provider.Id,
                ProviderName = service.Provider.Name
            };

            user.Balance -= charge;
            user.Spent += charge;

            _context.Orders.Add(order);
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            int orderId = order.Id;
            var newOrder = await _context.SmmServices
              .Include(s => s.externalServiceInfo)
              .Where(s => s.Id == service.Id)
              .Select(s => new AddOrderResponse
              {
                  apiurl = service.Provider.ApiUrl,
                  apikey = service.Provider.ApiKey,
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
                    .SetProperty(o => o.ProviderCharge, charge)
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
    }

    // ── DTOs ─────────────────────────────────────────────────────
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
