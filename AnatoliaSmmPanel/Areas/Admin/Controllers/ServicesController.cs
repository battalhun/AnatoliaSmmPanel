using AnatoliaSmmPanel.Areas.Admin.Data;
using AnatoliaSmmPanel.Areas.Admin.Dtos;
using AnatoliaSmmPanel.Areas.Admin.Models;
using AnatoliaSmmPanel.Areas.Admin.Services;
using AnatoliaSmmPanel.Areas.Admin.ViewModels.Services.Import;
using AnatoliaSmmPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace AnatoliaSmmPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("admin/[controller]/[action]")]
    [Route("admin/services")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ServicesController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly AdminContext _adminContext;
        private readonly ISmmApiService _smmApiService;


        public ServicesController(ILogger<SettingsController> logger, AdminContext adminContext, ISmmApiService smmApiService)
        {
            _logger = logger;
            _adminContext = adminContext;
            _smmApiService = smmApiService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            //int servicecount = await _adminContext.SmmServices.CountAsync();
            return View();
        }


        [HttpGet("services2")]
        public async Task<IActionResult> Services2()
        {
            int servicecount = await _adminContext.SmmServices.CountAsync();
            return View(servicecount);
        }

        [HttpGet("GetReduxAll")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetReduxAll()
        {
            var services = await _adminContext.SmmServices
                .Include(x => x.Provider)
                .Include(x => x.serviceCategory)
                .OrderBy(x => x.serviceCategoryId)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    Type = s.Type,
                    Rate = s.Rate,
                    Min = s.Min,
                    Max = s.Max,

                    ProviderId = s.ProviderId,
                    Provider = new ProviderViewModel
                    {
                        Id = s.Provider.Id,
                        Name = s.Provider.Name,
                        ApiUrl = s.Provider.ApiUrl,
                        ApiKey = s.Provider.ApiKey,
                        IsActive = s.Provider.IsActive
                    },

                    ServiceCategoryid = s.serviceCategoryId,
                    ServiceCategory = new ServiceCategory
                    {
                        Id = s.serviceCategory.Id,
                        Name = s.serviceCategory.Name,
                        IsActive = s.serviceCategory.IsActive,
                        SortOrder = s.serviceCategory.SortOrder
                    },

                    ExternalServiceInfoid = s.externalServiceInfoId,
                    externalServiceInfo = new ExternalServiceInfo
                    {
                        ExternalServiceId = s.externalServiceInfo.ExternalServiceId,
                        ExternalName = s.externalServiceInfo.ExternalName,
                        ExternalType = s.externalServiceInfo.ExternalType,
                        ExternalRate = s.externalServiceInfo.ExternalRate,
                        ExternalMin = s.externalServiceInfo.ExternalMin,
                        ExternalMax = s.externalServiceInfo.ExternalMax,
                        ExternalCategoryName = s.externalServiceInfo.ExternalCategoryName,
                    },
                    Dripfeed = s.Dripfeed,
                    Refill = s.Refill,
                    Cancel = s.Cancel,
                    IsActive = s.IsActive,
                })
                .ToListAsync();


            return Json(new
            {
                success = true,
                total = services.Count,
                data = services
            });
        }


        [HttpGet("import")]
        public IActionResult Import()
        {

            var providers = _adminContext.Providers
    .Where(p => p.IsActive)
    .Select(p => new ImportProviderViewModel
    {
        Id = p.Id,
        Name = p.Name
    })
    .ToList();

            var viewModel = new ImportViewModel
            {
                ımportProviderViewModel = providers,
                ImportServicesViewModel = new List<ImportServicesViewModel>()
            };

            return View(viewModel);
        }


        [HttpPost("import")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(int provider_id)
        {
            var provider = await _adminContext.Providers.FindAsync(provider_id);

            List<SmmServiceDto> externalServices =
                await _smmApiService.GetServicesAsync(provider.ApiUrl, provider.ApiKey);

            // category mapping
            var categories = externalServices
                .Select(x => x.Category)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            var categoryMap = categories
                .Select((name, index) => new ImportExternalServicesCategoryViewModel
                {
                    Id = index + 1,
                    Name = name
                })
                .ToDictionary(x => x.Name, x => x);

            // services mapping
            var services = externalServices
                .Select(service => new ImportServicesViewModel
                {
                    Id = service.Service,
                    Name = service.Name,
                    Type = service.Type,
                    Rate = service.Rate,
                    Min = service.Min,
                    Max = service.Max,
                    Dripfeed = service.Dripfeed,
                    Refill = service.Refill,
                    Cancel = service.Cancel,
                    ImportExternalServicesCategoryViewModelId = categoryMap.ContainsKey(service.Category) ? categoryMap[service.Category].Id : 0,
                    ImportExternalServicesCategoryViewModel = new ImportExternalServicesCategoryViewModel
                    {
                        Id = service.Service,
                        Name = service.Category
                    }

                })
                .ToList();

            var providers = _adminContext.Providers
  .Where(p => p.IsActive)
  .Select(p => new ImportProviderViewModel
  {
      Id = p.Id,
      Name = p.Name
  })
  .ToList();

            // ROOT MODEL
            var viewModel = new ImportViewModel
            {
                ImportServicesViewModel = services,
                ımportProviderViewModel = providers,
                SelectedProviderId = provider_id
            };

            return View(viewModel);
        }





        [HttpGet("ServicesTest")]
        public IActionResult ServicesTest()
        {
            return View();
        }


    }
}
