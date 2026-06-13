using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Data.Models.Admin;
using AnatoliaSmmPanel.Data.Models.Appliciton;
using AnatoliaSmmPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AnatoliaSmmPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/orders")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ISmmApiService _smmApiService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ILogger<OrdersController> logger, ApplicationDbContext context, ISmmApiService smmApiService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _smmApiService = smmApiService;
            _userManager = userManager;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // ── GET ALL (with status filter, search, pagination) ──────
        [HttpGet("GetAll")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetAll(string? status, int page = 1, int pageSize = 100, string? search = null, string? field = "all")
        {
            var query = _context.Orders
                .Include(o => o.Service)
                .Include(o => o.Provider)
                .Include(o => o.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(o => o.Status == status);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                switch (field)
                {
                    case "id":
                        if (int.TryParse(search, out var orderId))
                            query = query.Where(o => o.Id == orderId);
                        else
                            query = query.Where(o => false);
                        break;

                    case "link":
                        query = query.Where(o => o.Link != null && o.Link.Contains(search));
                        break;

                    case "user":
                        query = query.Where(o => o.User != null && o.User.Nickname != null && o.User.Nickname.Contains(search));
                        break;

                    case "service":
                        if (int.TryParse(search, out var serviceId))
                            query = query.Where(o => o.ServiceId == serviceId);
                        else
                            query = query.Where(o => false);
                        break;

                    default: // "all"
                        query = query.Where(o =>
                            o.Id.ToString().Contains(search) ||
                            (o.Link != null && o.Link.Contains(search)) ||
                            (o.User != null && o.User.Nickname != null && o.User.Nickname.Contains(search)) ||
                            (o.ExternalOrderId != null && o.ExternalOrderId.Contains(search)) ||
                            (o.ProviderName != null && o.ProviderName.Contains(search)));
                        break;
                }
            }

            var total = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new
                {
                    o.Id,
                    o.UserId,
                    UserNickname = o.User != null ? o.User.Nickname : null,
                    o.ServiceId,
                    ServiceName = o.Service != null ? o.Service.Name : null,
                    o.ProviderId,
                    o.Link,
                    o.Quantity,
                    o.StartCount,
                    o.Remains,
                    o.Charge,
                    o.ProviderCharge,
                    o.ExternalOrderId,
                    o.ProviderName,
                    o.Status,
                    o.Mode,
                    o.IsRefillEnabled,
                    o.CreatedAt,
                    o.UpdatedAt
                })
                .ToListAsync();

            return Json(new { success = true, total, data = orders });
        }

        // ── DETAILS ────────────────────────────────────────────────
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Service)
                .Include(o => o.Provider)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return PartialView("_OrderDetailsModal", order);
        }

        // ── SET START COUNT ───────────────────────────────────────
        [HttpGet("SetStartCount/{id}")]
        public async Task<IActionResult> SetStartCount(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            return PartialView("_SetStartCountModal", order);
        }

        [HttpPost("SetStartCount")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SetStartCount([FromBody] SetStartCountRequest request)
        {
            var order = await _context.Orders.FindAsync(request.Id);
            if (order == null) return Json(new { success = false, message = "Order not found" });

            order.StartCount = request.StartCount;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // ── SET PARTIAL ────────────────────────────────────────────
        [HttpGet("SetPartial/{id}")]
        public async Task<IActionResult> SetPartial(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            return PartialView("_SetPartialModal", order);
        }

        [HttpPost("SetPartial")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SetPartial([FromBody] SetPartialRequest request)
        {
            var order = await _context.Orders.FindAsync(request.Id);
            if (order == null) return Json(new { success = false, message = "Order not found" });

            if (request.Remains < 0 || request.Remains > order.Quantity)
                return Json(new { success = false, message = "Invalid remains value" });

            order.Remains = request.Remains;
            order.Status = "Partial";
            order.UpdatedAt = DateTime.UtcNow;

            var deliveredQuantity = order.Quantity - request.Remains;
            var unitPrice = order.Charge / order.Quantity;
            var newCharge = unitPrice * deliveredQuantity;
            var refundAmount = order.Charge - newCharge;

            if (refundAmount > 0)
            {
                var user = await _userManager.FindByIdAsync(order.UserId.ToString());
                if (user != null)
                {
                    user.Balance += refundAmount;
                    await _userManager.UpdateAsync(user);
                }
                order.Charge = newCharge;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, refundAmount });
        }

        // ── CHANGE STATUS (single) ────────────────────────────────
        [HttpPost("ChangeStatus")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeStatusRequest request)
        {
            var order = await _context.Orders.FindAsync(request.Id);
            if (order == null) return Json(new { success = false, message = "Order not found" });

            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // ── CANCEL + REFUND (single) ──────────────────────────────
        [HttpPost("CancelRefund")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CancelRefund([FromBody] CancelRefundRequest request)
        {
            var order = await _context.Orders.FindAsync(request.Id);
            if (order == null) return Json(new { success = false, message = "Order not found" });

            if (order.Status == "Canceled")
                return Json(new { success = false, message = "Order cannot be canceled" });

            var refundAmount = await RefundOrderAsync(order);

            return Json(new { success = true, refundAmount });
        }

        // Toplu durum değiştirme işlemi (örneğin: tüm seçilenleri "Completed" yap)
        [HttpPost("MassChangeStatus")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> MassChangeStatus([FromBody] MassChangeStatusRequest request)
        {
            if (request.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "No orders selected" });

            var orders = await _context.Orders
                .Where(o => request.Ids.Contains(o.Id))
                .ToListAsync();

            foreach (var order in orders)
            {
                order.Status = request.Status;
                order.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, updated = orders.Count });
        }

        // Toplu iptal + iade işlemi 
        [HttpPost("MassCancelRefund")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> MassCancelRefund([FromBody] MassCancelRefundRequest request)
        {
            if (request.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "No orders selected" });

            var orders = await _context.Orders
                .Where(o => request.Ids.Contains(o.Id)
                    && o.Status != "Canceled")
                .ToListAsync();

            decimal totalRefunded = 0;

            foreach (var order in orders)
            {
                totalRefunded += await RefundOrderAsync(order, save: false);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, canceled = orders.Count, totalRefunded });
        }

        // CSV olarak siparişleri dışa aktarma (isteğe bağlı durum filtresi ile)
        [HttpGet("Export")]
        public async Task<IActionResult> Export(string? status)
        {
            var query = _context.Orders
                .Include(o => o.Service)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(o => o.Status == status);

            var orders = await query
                .OrderByDescending(o => o.Id)
                .ToListAsync();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("ID,User,Charge,Link,Start Count,Quantity,Service,Status,Remains,Created,Mode");

            foreach (var o in orders)
            {
                sb.AppendLine(string.Join(",",
                    o.Id,
                    Csv(o.User?.Nickname),
                    o.Charge.ToString(CultureInfo.InvariantCulture),
                    Csv(o.Link),
                    o.StartCount,
                    o.Quantity,
                    Csv(o.Service?.Name),
                    Csv(o.Status),
                    o.Remains,
                    o.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    Csv(o.Mode)
                ));
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", $"orders_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv");
        }

        // Helper metodu: Bir siparişi iptal edip iade işlemi yapar, isterseniz değişiklikleri kaydetmeden sadece toplam iade miktarını döndürebilir
        private async Task<decimal> RefundOrderAsync(Order order, bool save = true)
        {
            var user = await _userManager.FindByIdAsync(order.UserId.ToString());
            decimal refundAmount = 0;

            if (user != null)
            {
                refundAmount = order.Charge;
                user.Balance += refundAmount;
                user.Spent -= refundAmount;
                await _userManager.UpdateAsync(user);
            }

            order.Status = "Canceled";
            order.Remains = order.Quantity;
            order.UpdatedAt = DateTime.UtcNow;

            if (save)
                await _context.SaveChangesAsync();

            return refundAmount;
        }

        // Helper metodu: CSV formatında değerleri escaping yapar
        private static string Csv(string? value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            return value;
        }
    }

    // ── REQUEST DTOs ───────────────────────────────────────────────
    public class SetStartCountRequest
    {
        public int Id { get; set; }
        public int StartCount { get; set; }
    }

    public class SetPartialRequest
    {
        public int Id { get; set; }
        public int Remains { get; set; }
    }

    public class ChangeStatusRequest
    {
        public int Id { get; set; }
        public string Status { get; set; } = "";
    }

    public class CancelRefundRequest
    {
        public int Id { get; set; }
    }

    public class MassChangeStatusRequest
    {
        public List<int> Ids { get; set; } = new();
        public string Status { get; set; } = "";
    }

    public class MassCancelRefundRequest
    {
        public List<int> Ids { get; set; } = new();
    }
}
