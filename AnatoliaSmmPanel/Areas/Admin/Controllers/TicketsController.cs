using AnatoliaSmmPanel.Areas.Admin.Enums;
using AnatoliaSmmPanel.Areas.Admin.ViewModels.Tickets;
using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Data.Models.Admin;
using AnatoliaSmmPanel.Data.Models.Appliciton;
using AnatoliaSmmPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AnatoliaSmmPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/tickets")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TicketsController : Controller
    {
        private readonly ILogger<TicketsController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ISmmApiService _smmApiService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketsController(ILogger<TicketsController> logger, ApplicationDbContext context, ISmmApiService smmApiService, UserManager<ApplicationUser> userManager)
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


        [HttpGet("GetAll")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetAll(string? status, int page = 1, int pageSize = 100, string? search = null, string? field = "all")
        {
            var query = _context.Tickets.AsQueryable();

            // 1. Statü Filtresi
            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                if (Enum.TryParse<TicketStatusEnum>(status, true, out var parsedStatus))
                    query = query.Where(t => t.Status == parsedStatus);
                else
                    query = query.Where(t => false);
            }

            // 2. Arama Filtresi
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                List<int> matchedUserIds = new List<int>(); // Artık int tutuyoruz

                // Arama kısmında User tablosundaki int tipindeki UserId'leri çekiyoruz
                if (field == "user" || field == "all")
                {
                    matchedUserIds = await _userManager.Users
                        .Where(u => u.Nickname != null && u.Nickname.Contains(search))
                        .Select(u => u.UserId) // Guid olan Id'yi değil, int olan UserId'yi alıyoruz
                        .ToListAsync();
                }

                switch (field)
                {
                    case "id":
                        if (int.TryParse(search, out var ticketId))
                            query = query.Where(t => t.Id == ticketId);
                        else
                            query = query.Where(t => false);
                        break;

                    case "user":
                        query = query.Where(t => matchedUserIds.Contains(t.UserId));
                        break;

                    case "subject":
                        query = query.Where(t => t.Subject != null && t.Subject.Contains(search));
                        break;

                    default: // "all"
                        bool isNumeric = int.TryParse(search, out var searchId);
                        query = query.Where(t =>
                            (isNumeric && t.Id == searchId) ||
                            (t.Subject != null && t.Subject.Contains(search)) ||
                            matchedUserIds.Contains(t.UserId)
                        );
                        break;
                }
            }

            var total = await query.CountAsync();

            // 3. Veriyi Çekme
            var pagedTickets = await query
                .OrderByDescending(t => t.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 4. İlgili sayfadaki int UserId'leri topla
            var userIdsInPage = pagedTickets.Select(t => t.UserId).Distinct().ToList();

            // 5. UserManager üzerinden int UserId ile eşleşenleri getir
            var usersDict = await _userManager.Users
                .Where(u => userIdsInPage.Contains(u.UserId)) // Guid Id yerine int UserId kullanıyoruz
                .Select(u => new { u.UserId, u.Nickname })
                .ToDictionaryAsync(u => u.UserId, u => u.Nickname);

            // 6. View Model Eşleştirmesi
            var resultList = pagedTickets.Select(t => new TicketsViewModel
            {
                Id = t.Id,
                UserId = t.UserId,
                UserNickname = usersDict.ContainsKey(t.UserId) ? usersDict[t.UserId] : null,
                Subject = t.Subject,
                Status = t.Status.ToString(),
                IsUnread = t.IsUnread,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList();

            return Json(new { success = true, total = total, data = resultList });
        }

        // Ticket detaylarını getiren endpoint
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
                return NotFound("No tickets found."); 
  
            var user = await _userManager.Users
                .Where(u => u.UserId == ticket.UserId)
                .Select(u => new { u.UserId, u.Nickname })
                .FirstOrDefaultAsync();

          
            var messages = await _context.TicketMessages 
                .Where(m => m.TicketId == id)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

         
            var viewModel = new TicketDetailsViewModel
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                UserNickname = user?.Nickname,
                Subject = ticket.Subject,
                Status = ticket.Status, 
                IsUnread = ticket.IsUnread,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                Messages = messages 
            };
            return PartialView("_TicketDetailsModal", viewModel);
        }

        // ── REPLY TO TICKET ─────────────────────────────────────────
        [HttpPost("Reply")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Reply([FromBody] TicketReplyRequest request)
        {
            // 1. Gelen isteğin boş olup olmadığını kontrol edelim
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
                return Json(new { success = false, message = "Message cannot be empty." });

            var ticket = await _context.Tickets.FindAsync(request.TicketId);
            if (ticket == null)
                return Json(new { success = false, message = "Ticket not found." });

            // 2. Oturum açmış kullanıcının Identity Id'sini (Guid string) alıyoruz
            var currentIdentityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentIdentityId))
                return Json(new { success = false, message = "User is not authenticated." });

            // 3. UserManager'dan kullanıcıyı bulup int tipindeki UserId'sine ulaşıyoruz
            var currentUser = await _userManager.FindByIdAsync(currentIdentityId);
            if (currentUser == null)
                return Json(new { success = false, message = "User details could not be found." });

            // 4. Ticket mesajını oluşturuyoruz
            var newMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                UserId = currentUser.UserId, // int.Parse() yerine veritabanındaki int alanı kullanıyoruz
                Message = request.Message,
                IsAdmin = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.TicketMessages.Add(newMessage);

            ticket.Status = TicketStatusEnum.Answered;
            ticket.UpdatedAt = DateTime.UtcNow; // Opsiyonel: t.IsUnread = true (Kullanıcı için okunmadı olarak işaretlenebilir)

            // EF Core iki işlemi de tek bir transaction içinde güvenle kaydeder
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Reply sent successfully." });
        }

        // ── CHANGE STATUS (Close, Reopen vb.) ───────────────────────
        [HttpPost("ChangeStatus")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeTicketStatusRequest request)
        {
            var ticket = await _context.Tickets.FindAsync(request.Id);
            if (ticket == null) return Json(new { success = false, message = "Ticket not found." });

            ticket.Status = request.Status;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // ── MASS CHANGE STATUS ──────────────────────────────────────
        [HttpPost("MassChangeStatus")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> MassChangeStatus([FromBody] MassChangeTicketStatusRequest request)
        {
            if (request.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "No tickets selected." });

            var tickets = await _context.Tickets
                .Where(t => request.Ids.Contains(t.Id))
                .ToListAsync();

            foreach (var ticket in tickets)
            {
                ticket.Status = request.Status;
                ticket.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, updated = tickets.Count });
        }

        // ── EXPORT TO CSV ───────────────────────────────────────────
        [HttpGet("Export")]
        public async Task<IActionResult> Export(string? status, string? search = null, string? field = "all")
        {
            var query = _context.Tickets
                .Include(t => t.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                if (Enum.TryParse<TicketStatusEnum>(status, true, out var parsedStatus))
                    query = query.Where(t => t.Status == parsedStatus);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                switch (field)
                {
                    case "id":
                        if (int.TryParse(search, out var ticketId)) query = query.Where(t => t.Id == ticketId);
                        break;
                    case "user":
                        query = query.Where(t => t.User != null && t.User.Nickname != null && t.User.Nickname.Contains(search));
                        break;
                    case "subject":
                        query = query.Where(t => t.Subject != null && t.Subject.Contains(search));
                        break;
                    default:
                        query = query.Where(t => t.Id.ToString().Contains(search) ||
                            (t.User != null && t.User.Nickname != null && t.User.Nickname.Contains(search)) ||
                            (t.Subject != null && t.Subject.Contains(search)));
                        break;
                }
            }

            var tickets = await query.OrderByDescending(t => t.Id).ToListAsync();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("ID,User,Subject,Status,Created At,Last Update");

            foreach (var t in tickets)
            {
                var subject = t.Subject?.Replace("\"", "\"\"").Replace("\n", " ") ?? "";
                var user = t.User?.Nickname ?? "";

                sb.AppendLine($"{t.Id},\"{user}\",\"{subject}\",{t.Status},{t.CreatedAt:yyyy-MM-dd HH:mm},{t.UpdatedAt:yyyy-MM-dd HH:mm}");
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            var result = new byte[bytes.Length + 3];
            result[0] = 0xEF; result[1] = 0xBB; result[2] = 0xBF;
            Array.Copy(bytes, 0, result, 3, bytes.Length);

            return File(result, "text/csv", $"tickets_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv");
        }

        // ── CREATE TICKET MODAL (GET) ───────────────────────────────
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return PartialView("_NewTicketModal");
        }

        // ── CREATE TICKET (POST) ────────────────────────────────────
        [HttpPost("create")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Create([FromBody] NewTicketViewModel model)
        {
            if (model == null)
                return Json(new { success = false, message = "Invalid request." });

            if (model.UserId <= 0)
                return Json(new { success = false, message = "User Id is required." });

            if (string.IsNullOrWhiteSpace(model.Subject))
                return Json(new { success = false, message = "Subject is required." });

            if (string.IsNullOrWhiteSpace(model.Message))
                return Json(new { success = false, message = "Message is required." });

            var userExists = await _context.Users.AnyAsync(u => u.UserId == model.UserId);
            if (!userExists)
                return Json(new { success = false, message = "User not found." });

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ticket = new Ticket
            {
                UserId = model.UserId,
                Subject = model.Subject,
                Status = TicketStatusEnum.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsUnread = false,
                Messages = new List<TicketMessage>
                {
                    new TicketMessage
                    {
                        UserId = int.Parse(currentUserId!),
                        Message = model.Message,
                        IsAdmin = true,
                        CreatedAt = DateTime.UtcNow
                    }
                }
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Json(new { success = true, id = ticket.Id });
        }
    }

    // ── REQUEST DTOs ───────────────────────────────────────────────
    public class TicketReplyRequest
    {
        public int TicketId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ChangeTicketStatusRequest
    {
        public int Id { get; set; }
        public TicketStatusEnum Status { get; set; }
    }

    public class MassChangeTicketStatusRequest
    {
        public List<int> Ids { get; set; } = new();
        public TicketStatusEnum Status { get; set; }
    }
}