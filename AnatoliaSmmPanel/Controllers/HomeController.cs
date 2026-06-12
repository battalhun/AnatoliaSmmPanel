using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AnatoliaSmmPanel.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }



        [Authorize]
        [HttpGet("new-order")]
        public IActionResult NewOrder()
        {
            return View();
        }

       
    }
}
