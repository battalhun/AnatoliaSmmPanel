using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Data.Models.Appliciton;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnatoliaSmmPanel.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }



        [Authorize]
        [HttpGet("new-order")]
        public IActionResult NewOrder()
        {
            var userId = _userManager.GetUserId(User);
            
            var balance = _userManager.Users.Where(u => u.Id == userId).Select(u => u.Balance).FirstOrDefault();
            var spent = _userManager.Users.Where(u => u.Id == userId).Select(u => u.Spent).FirstOrDefault();
            ViewData["Balance"] = balance;
            ViewData["Spent"] = spent;


            return View();
        }

        [Authorize]
        [HttpGet("services")]
        public IActionResult Services()
        {
            return View();
        }

        [Authorize]
        [HttpGet("tickets")]
        public IActionResult Tickets()
        {
            return View();
        }

    }
}
