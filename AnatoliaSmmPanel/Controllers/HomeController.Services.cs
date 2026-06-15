using AnatoliaSmmPanel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnatoliaSmmPanel.Controllers
{
    //  HomeController.Services.cs
    public partial class HomeController
    {
        // Services Sayfası
        [Authorize]
        [HttpGet("services")]
        public IActionResult Services()
        {
            List<HomeServicesViewModel> services = _context.SmmServices
                .Include(s => s.serviceCategory)
                .Where(s => s.IsActive)
                .OrderBy(s => s.SortOrder)
                .Select(s => new HomeServicesViewModel
                {
                    Id = s.Id,
                    CategoryId = s.serviceCategoryId ?? 0,
                    CategoryName = s.serviceCategory != null ? s.serviceCategory.Name : "General",
                    Name = s.Name,
                    Rate = s.Rate,
                    Min = s.Min,
                    Max = s.Max,
                    Type = s.Type,
                    //    Description = s.Description,
                    Dripfeed = s.Dripfeed,
                    Refill = s.Refill,
                    Cancel = s.Cancel
                })
                .ToList();

            return View(services);
        }
    }
}
