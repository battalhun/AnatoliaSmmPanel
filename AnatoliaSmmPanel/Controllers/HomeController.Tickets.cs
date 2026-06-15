using AnatoliaSmmPanel.Areas.Admin.Enums;
using AnatoliaSmmPanel.Data.Models.Admin;
using AnatoliaSmmPanel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnatoliaSmmPanel.Controllers
{
    // HomeController.Tickets.cs
    public partial class HomeController
    {
        // Tickets Sayfası
        [Authorize]
        [HttpGet("tickets")]
        public async Task<IActionResult> Tickets()
        {
            var intUserId = GetIntUserId();

            var tickets = await _context.Tickets
                .Where(t => t.UserId == intUserId)
                .OrderByDescending(t => t.UpdatedAt)
                .Select(t => new HomeTicketListItemViewModel
                {
                    Id = t.Id,
                    Subject = t.Subject,
                    Status = t.Status.ToString(),
                    IsUnread = t.IsUnread,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync();

            return View(tickets);
        }

        // Yeni Ticket oluşturma
        [Authorize]
        [HttpPost("home/tickets/create")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateTicket([FromBody] CreateHomeTicketRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Subject))
                return Json(new { success = false, message = "Subject is required." });

            if (string.IsNullOrWhiteSpace(request.Message))
                return Json(new { success = false, message = "Message is required." });

            var intUserId = GetIntUserId();

            var ticket = new Ticket
            {
                UserId = intUserId,
                Subject = request.Subject.Trim(),
                Status = TicketStatusEnum.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsUnread = true,
                Messages = new List<TicketMessage>
                {
                    new TicketMessage
                    {
                        UserId = intUserId,
                        Message = request.Message.Trim(),
                        IsAdmin = false,
                        CreatedAt = DateTime.UtcNow
                    }
                }
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Json(new { success = true, id = ticket.Id });
        }

        // Ticket Detayları
        [Authorize]
        [HttpGet("home/tickets/details/{id}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> TicketDetails(int id)
        {
            var intUserId = GetIntUserId();

            var ticket = await _context.Tickets
                .Include(t => t.Messages.OrderBy(m => m.CreatedAt))
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == intUserId);

            if (ticket == null)
                return Json(new { success = false, message = "Ticket not found." });

            if (ticket.IsUnread)
            {
                ticket.IsUnread = false;
                await _context.SaveChangesAsync();
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    id = ticket.Id,
                    subject = ticket.Subject,
                    status = ticket.Status.ToString(),
                    messages = ticket.Messages
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => new
                        {
                            message = m.Message,
                            isAdmin = m.IsAdmin,
                            createdAt = m.CreatedAt
                        })
                }
            });
        }

        // Ticket Mesaj gönderme
        [Authorize]
        [HttpPost("home/tickets/reply")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ReplyToTicket([FromBody] HomeTicketReplyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return Json(new { success = false, message = "Message cannot be empty." });

            var intUserId = GetIntUserId();

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == request.TicketId && t.UserId == intUserId);

            if (ticket == null)
                return Json(new { success = false, message = "Ticket not found." });

            if (ticket.Status == TicketStatusEnum.Closed)
                return Json(new { success = false, message = "This ticket is closed." });

            _context.TicketMessages.Add(new TicketMessage
            {
                TicketId = ticket.Id,
                UserId = intUserId,
                Message = request.Message.Trim(),
                IsAdmin = false,
                CreatedAt = DateTime.UtcNow
            });

            ticket.Status = TicketStatusEnum.Pending;
            ticket.IsUnread = true;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }

    // ── DTOs ─────────────────────────────────────────────────────
    public class CreateHomeTicketRequest
    {
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class HomeTicketReplyRequest
    {
        public int TicketId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
