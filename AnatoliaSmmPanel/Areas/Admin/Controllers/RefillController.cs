using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Data.Models.Appliciton;
using AnatoliaSmmPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnatoliaSmmPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/refill")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class RefillController : Controller
    {
        private readonly ILogger<RefillController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ISmmApiService _smmApiService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RefillController(ILogger<RefillController> logger, ApplicationDbContext context, ISmmApiService smmApiService, UserManager<ApplicationUser> userManager)
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
    }
}
