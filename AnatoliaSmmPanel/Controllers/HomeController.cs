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
        private readonly HomeContext _homeContext;

        public HomeController(ILogger<HomeController> logger, HomeContext homeContext)
        {
            _logger = logger;
            _homeContext = homeContext;
        }



        [Authorize]
        [HttpGet("new-order")]
        public IActionResult NewOrder()
        {
            return View();
        }

       
    }
}
