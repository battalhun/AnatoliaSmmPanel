using AnatoliaSmmPanel.Areas.Admin.Data;
using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Data.Models.Appliciton;
using AnatoliaSmmPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("GetAll")]
        [IgnoreAntiforgeryToken]
        public IActionResult GetAll()
        {
            var orders = _context.Orders
                .OrderBy(x => x.Id)
                //.Include(o => o.Service)
                //.Include(o => o.Provider)
                //.Include(o => o.User) // <--- HATA BURADAYDI, UserId yerine User yazdık
                .ToList();

            return Json(new { success = true, total = orders.Count, data = orders });
        }
    }
}
