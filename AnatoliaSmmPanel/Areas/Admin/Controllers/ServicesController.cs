using AnatoliaSmmPanel.Areas.Admin.Data;
using AnatoliaSmmPanel.Areas.Admin.Dtos;
using AnatoliaSmmPanel.Areas.Admin.Services;
using AnatoliaSmmPanel.Areas.Admin.ViewModels.Services;
using AnatoliaSmmPanel.Areas.Admin.ViewModels.Services.Import;
using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Data.Models.Admin;
using AnatoliaSmmPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AnatoliaSmmPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/services")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ServicesController : Controller
    {
        private readonly ILogger<ServicesController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ISmmApiService _smmApiService;


        public ServicesController(ILogger<ServicesController> logger, ApplicationDbContext context, ISmmApiService smmApiService)
        {
            _logger = logger;
            _context = context;
            _smmApiService = smmApiService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var providers = _context.Providers
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

        [HttpGet("GetReduxAll")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetReduxAll()
        {
            var services = await _context.SmmServices
                .Where(s => !s.IsDeleted)
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
                    ServiceCategory = s.serviceCategory == null ? null : new ServiceCategory
                    {
                        Id = s.serviceCategory.Id,
                        Name = s.serviceCategory.Name,
                        IsActive = s.serviceCategory.IsActive,
                        SortOrder = s.serviceCategory.SortOrder
                    },

                    ExternalServiceInfoid = s.externalServiceInfoId,
                    externalServiceInfo = s.externalServiceInfo == null ? null : new ExternalServiceInfo
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

            var providers = _context.Providers
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
            var provider = await _context.Providers.FindAsync(provider_id);

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

            var providers = _context.Providers
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




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportSelectedServices(ImportSelectedServicesViewModel model)
        {
            if (model.SelectedProviderId == 0 || !model.SelectedServiceIds.Any())
            {
                return BadRequest();
            }

            var provider = await _context.Providers.FindAsync(model.SelectedProviderId);

            List<SmmServiceDto> externalServices =
               await _smmApiService.GetServicesAsync(provider.ApiUrl, provider.ApiKey);

            foreach (var serviceId in model.SelectedServiceIds)
            {
                var externalService = externalServices.FirstOrDefault(s => s.Service == serviceId);
                if (externalService != null)
                {
                    // Fiyatı güvenli bir şekilde parse etme
                    decimal parsedRate = 0;
                    decimal.TryParse(externalService.Rate,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out parsedRate);

                    SmmService newService = new SmmService
                    {
                        Name = externalService.Name,
                        Type = externalService.Type,
                        Rate = parsedRate,
                        Min = externalService.Min,
                        Max = externalService.Max,
                        Dripfeed = externalService.Dripfeed,
                        Refill = externalService.Refill,
                        Cancel = externalService.Cancel,
                        ProviderId = model.SelectedProviderId,
                        serviceCategoryId = null,

                        // externalServiceInfoId alanını YAZMIYORUZ, EF Core otomatik halledecek.

                        externalServiceInfo = new ExternalServiceInfo
                        {
                            ExternalServiceId = externalService.Service.ToString(), // İleride Sync yapabilmek için çok önemli!
                            ExternalName = externalService.Name,
                            ExternalType = externalService.Type,
                            ExternalMin = externalService.Min,
                            ExternalMax = externalService.Max,
                            ExternalRate = parsedRate, // Karşı tarafın orijinal fiyatını da tutmak faydalıdır
                            ExternalCategoryName = externalService.Category,
                            LastSyncAt = DateTime.UtcNow, // İlk eklenme anını sync zamanı olarak alabiliriz
                           
                        },
                        IsActive = true
                    };

                    _context.SmmServices.Add(newService);
                }
            }

            // SaveChangesAsync çağrıldığında EF Core her iki tabloya da veriyi ekler 
            // ve aralarındaki ID ilişkisini kusursuzca kurar.
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                providerId = model.SelectedProviderId,
                count = model.SelectedServiceIds.Count
            });
        }



        [HttpGet("AddCategory")]
        public IActionResult AddCategory()
        {
            return PartialView("_AddCategoryModal");
        }

        [HttpPost("CreateCategory")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                return Json(new { success = false, message = "Category name is required." });

            var exists = await _context.ServiceCategories
                .AnyAsync(x => x.Name == model.Name);

            if (exists)
                return Json(new { success = false, message = "A category with this name already exists." });

            var category = new ServiceCategory
            {
                Name = model.Name.Trim(),
                SortOrder = model.SortOrder,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.ServiceCategories.Add(category);
            await _context.SaveChangesAsync();

            return Json(new { success = true, id = category.Id, name = category.Name });
        }





        [HttpGet("EditService/{id}")]
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _context.SmmServices
                .Include(x => x.serviceCategory)
                .Include(x => x.Provider)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (service == null) return NotFound();

            var vm = new EditServiceViewModel
            {
                Id = service.Id,
                Name = service.Name,
                Type = service.Type,
                Rate = service.Rate,
                Min = service.Min,
                Max = service.Max,
                SortOrder = service.SortOrder,
                Dripfeed = service.Dripfeed,
                Refill = service.Refill,
                Cancel = service.Cancel,
                IsActive = service.IsActive,
                ProviderId = service.ProviderId,
                ServiceCategoryId = service.serviceCategoryId
            };

            return PartialView("_EditServiceModal", vm);
        }


        [HttpPost("UpdateService")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateService([FromBody] EditServiceViewModel model)
        {
            var service = await _context.SmmServices.FindAsync(model.Id);

            if (service == null)
                return Json(new { success = false, message = "Service not found." });

            service.Name = model.Name.Trim();
            service.Type = model.Type?.Trim();
            service.Rate = model.Rate;
            service.Min = model.Min;
            service.Max = model.Max;
            service.SortOrder = model.SortOrder;
            service.serviceCategoryId = model.ServiceCategoryId;
            service.ProviderId = model.ProviderId;
            service.Dripfeed = model.Dripfeed;
            service.Refill = model.Refill;
            service.Cancel = model.Cancel;
            service.IsActive = model.IsActive;
            service.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }





        #region Servis aktif etme işlemleri
        // GetDisableService Servislere IsActive = False etmek için kullanılır.
        [HttpPost("GetDisableService/{id}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetDisableService(int id)
        {
            var service = await _context.SmmServices.FindAsync(id);
            if (service == null)
                return Json(new { success = false, message = "Service not found." });

            service.IsActive = false;
            await _context.SaveChangesAsync();
            return Json(new { success = true });

        }

        // GetEnabledService Servislere IsActive = True etmek için kullanılır.
        [HttpPost("GetEnableServices/{id}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetEnableServices(int id)
        {
            var service = await _context.SmmServices.FindAsync(id);
            if (service == null)
                return Json(new { success = false, message = "Service not found." });

            service.IsActive = true;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }



        // BulkSetActive Servislere toplu olarak IsActive = True veya False etmek için kullanılır.
        [HttpPost("BulkSetActive")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> BulkSetActive([FromBody] BulkSetActiveRequest request)
        {
            if (request.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "No services selected." });

            var services = await _context.SmmServices
                .Where(s => request.Ids.Contains(s.Id))
                .ToListAsync();

            foreach (var service in services)
            {
                service.IsActive = request.IsActive;
                service.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, updated = services.Count });
        } 
        #endregion


        #region Servis silme işlemleri
        // DeletedServices Silinmiş servisleri listelemek için kullanılır.
        [HttpGet("DeletedServices")]
        public async Task<IActionResult> DeletedServices()
        {
            List<DeletedViewModel> services = await _context.SmmServices
                .Where(s => s.IsDeleted)
                .Select(s => new DeletedViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsActive = s.IsActive,
                    IsDeleted = s.IsDeleted
                }).ToListAsync();


            return View(services);
        }



       
        // GetDeletedService Servislere IsDeleted = True etmek için kullanılır.
        [HttpPost("GetDeletedService/{id}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetDeletedService(int id)
        {
            var service = await _context.SmmServices.FindAsync(id);
            if (service == null)
                return Json(new { success = false, message = "Service not found." });
            service.IsActive = false;
            service.IsDeleted = true;
            service.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // GetRestoreService Servislere IsDeleted = False etmek için kullanılır.
        [HttpPost("GetRestoreService/{id}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetRestoreService(int id)
        {
            var service = await _context.SmmServices.FindAsync(id);
            if (service == null)
                return Json(new { success = false, message = "Service not found." });
            service.IsActive = false;
            service.IsDeleted = false;
            service.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        } 
        #endregion

    }

    public class BulkSetActiveRequest
    {
        public List<int> Ids { get; set; } = new();
        public bool IsActive { get; set; }
    }


}
