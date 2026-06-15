using AnatoliaSmmPanel.Areas.Admin.Dtos;
using AnatoliaSmmPanel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnatoliaSmmPanel.Controllers
{
    // HomeController.Orders.cs 
    public partial class HomeController
    {
        // Müşteri orders sayfası
        [Authorize]
        [HttpGet("orders")]
        [HttpGet("orders/{status}")]
        [HttpGet("orders/{status}/{page:int}")]
        public async Task<IActionResult> Orders(string status = "all", int page = 1, string? search = null)
        {
            var intUserId = GetIntUserId();

            const int pageSize = 20;

            var query = _context.Orders
                .Include(o => o.Service)
                .Where(o => o.UserId == intUserId);

            // Statüs filtresi
            if (!string.IsNullOrEmpty(status) && status.ToLower() != "all")
            {
                var statusMap = new Dictionary<string, string>
                {
                    ["pending"] = "Pending",
                    ["inprogress"] = "In progress",
                    ["completed"] = "Completed",
                    ["partial"] = "Partial",
                    ["processing"] = "Processing",
                    ["canceled"] = "Canceled",
                };

                if (statusMap.TryGetValue(status.ToLower(), out var mappedStatus))
                {
                    query = query.Where(o => o.Status == mappedStatus);
                }
            }

            // Arama Textbox
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();

                if (int.TryParse(s, out var orderId))
                {
                    query = query.Where(o => o.Id == orderId);
                }
                else
                {
                    query = query.Where(o =>
                        o.Link.Contains(s) ||
                        (o.Service != null && o.Service.Name.Contains(s)));
                }
            }

            var totalCount = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new HomeOrderListItemViewModel
                {
                    Id = o.Id,
                    ServiceId = o.ServiceId,
                    ServiceName = o.Service != null ? o.Service.Name : "",
                    Link = o.Link,
                    Quantity = o.Quantity,
                    StartCount = o.StartCount,
                    Remains = o.Remains,
                    Charge = o.Charge,
                    Status = o.Status,
                    RefillAvailable = o.Service != null && o.Service.Refill
                                      && o.Status == "Completed"
                                      && !o.IsRefillEnabled,
                    CancelAvailable = o.Service != null && o.Service.Cancel
                                      && (o.Status == "Pending" || o.Status == "In progress" || o.Status == "Processing"),
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync();

            var viewModel = new HomeOrdersPageViewModel
            {
                Orders = orders,
                Status = status?.ToLower() ?? "all",
                Search = search,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return View(viewModel);
        }

        // Cancel butonu
        [Authorize]
        [HttpPost("home/orders/cancel")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CancelOrder([FromBody] CancelOrderRequest request)
        {
            var intUserId = GetIntUserId();

            var order = await _context.Orders
                .Include(o => o.Service)
                .Include(o => o.Provider)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == intUserId);

            if (order == null)
                return Json(new { success = false, message = "Order not found." });

            if (order.Service == null || !order.Service.Cancel)
                return Json(new { success = false, message = "This order cannot be canceled." });

            if (order.Status != "Pending" && order.Status != "In progress" && order.Status != "Processing")
                return Json(new { success = false, message = "This order is no longer eligible for cancellation." });

            if (order.Provider == null || string.IsNullOrEmpty(order.ExternalOrderId))
                return Json(new { success = false, message = "This order has no provider reference and cannot be canceled automatically." });

            List<SmmCancelOrderResponseDto> apiResponse = await _smmApiService.AddCancelOrderAsync(
                order.Provider.ApiUrl,
                order.Provider.ApiKey,
                new List<string> { order.ExternalOrderId },
                HttpMethod.Post
            );

            var result = apiResponse?.FirstOrDefault();

            if (result != null && result.IsSuccess)
            {
                string CancelOrders = result.Cancel.ToString(); // Apiden Gelen Cancel Sipariş ID'lerini String Olarak Alıyoruz
                // İleride Cancel Tablosu Eklendiğinde bu ID'ler Cancel Tablosuna Kaydedilecek. Provider Cancel Onaylaması Durumunda
                // Cancel Durumu Completed olacak ve orders tablosu duruma göre Partial veya Canceled olarak güncellenecek.
                // Şimdilik API'den Cancel Onayı Geldiği An Siparişi Canceled Yapıyoruz.
                // Çünkü Provider cancel id döndüğünde aslında direkt iptal etmiyor, sadece işleme alıyor,
                // provider bu talebi onaylayabilir veya reddedebilir.

                order.Status = "Canceled";
                order.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                order.Status = "Fail";
                // Admin Panel için fail olarak gösterilecek, müşteri tarafı için pending olarak ayarlanacak.
                order.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Json(new { success = false, message = result?.ErrorMessage ?? "Unknown API error occurred." });
            }
        }
    }

    // ── DTO ────────────────────────────────────────────────────
    public class CancelOrderRequest
    {
        public int OrderId { get; set; }
    }
}
