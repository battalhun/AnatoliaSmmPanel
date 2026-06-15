using AnatoliaSmmPanel.Areas.Admin.Services;
using AnatoliaSmmPanel.Areas.Admin.ViewModels;
using AnatoliaSmmPanel.Data.Models.Admin;
using AnatoliaSmmPanel.Data;
using AnatoliaSmmPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AnatoliaSmmPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SettingsController : Controller
    {

        private readonly ILogger<SettingsController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ISmmApiService _smmApiService;
        private readonly ISettingsService _settingsService;

        public SettingsController(ILogger<SettingsController> logger, ApplicationDbContext context, ISmmApiService smmApiService, ISettingsService settingsService)
        {
            _logger = logger;
            _context = context;
            _smmApiService = smmApiService;
            _settingsService = settingsService;
        }



        [Authorize(Roles = "SuperAdmin,SettingsAdmin")]
        [HttpGet("{section?}")]
        public async Task<IActionResult> Index(string section = "general")
        {
            var viewModel = new SettingsViewModel();


            if (section.ToLower() == "general")
                viewModel.General = await _settingsService.GetSettingsAsync();

            else if (section.ToLower() == "providers")
            {
                viewModel.Providers = _context.Providers.Select(x => new ProviderViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    AliasEnabled = x.AliasEnabled,
                    Alias = x.Alias,
                    LowBalanceNotificationEnabled = x.LowBalanceNotificationEnabled,
                    CurrentBalance = x.CurrentBalance
                }).ToList();
            }

            return View(viewModel);
        }

        [HttpPost("providers/addprovider")]
        public async Task<IActionResult> AddProvider(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                TempData["Error"] = "Lütfen geçerli bir url girin.";
                TempData["AddProviderModal"] = true;
                return RedirectToAction("Index", new { section = "providers" });
            }

            // 1. Linki Normalize Et
            var normalized = NormalizeProviderUrl(url);
            var apiUrl = normalized + "/api/v2";

            // 2. Bu sağlayıcı zaten veritabanımızda kayıtlı mı kontrolü 
            bool isExist = _context.Providers.Any(p => p.ApiUrl == apiUrl);
            if (isExist)
            {
                TempData["Error"] = "Bu sağlayıcı zaten mevcut.";
                TempData["AddProviderModal"] = true;
                return RedirectToAction("Index", new { section = "providers" });
            }

            // 3. Gerçek bir SMM API'si olup olmadığını test et
            bool isValidApi = await _smmApiService.IsValidSmmApiAsync(apiUrl);
            if (!isValidApi)
            {
                TempData["Error"] = "Girilen URL geçerli bir SMM Panel API'sine ait değil veya ulaşılamıyor.";
                TempData["AddProviderModal"] = true;
                return RedirectToAction("Index", new { section = "providers" });
            }

            // 4. Her şey yolundaysa DB’ye kaydet
            var provider = new Provider
            {
                Name = ExtractDomain(normalized),
                ApiUrl = apiUrl,
                AliasEnabled = true,
                Alias = ExtractDomain(normalized),
                LowBalanceNotificationEnabled = false,
                BalanceLimit = 0,
                Currency = "USD",
                IsActive = true
            };


            _context.Providers.Add(provider);
            await _context.SaveChangesAsync(); // Async kullandığımız için SaveChangesAsync daha iyi olur

            TempData["Success"] = "Sağlayıcı başarıyla doğrulandı ve eklendi!";

            TempData["EditProviderModal"] = true;
            TempData["EditProviderId"] = provider.Id;

            return RedirectToAction("Index", new { section = "providers" });
        }
       
        private string NormalizeProviderUrl(string input)
        {
            input = input.Trim();

            // Eğer http/https yoksa ekle
            if (!input.StartsWith("http://") && !input.StartsWith("https://"))
            {
                input = "https://" + input;
            }

            var uri = new Uri(input);

            // sadece domain al (path temizlenir)
            var baseUrl = uri.Scheme + "://" + uri.Host;

            return baseUrl;
        }

        private string ExtractDomain(string url)
        {
            var uri = new Uri(url);
            return uri.Host.Replace("www.", "");
        }

        [HttpPost("providers/delete/{id}")]
        public IActionResult DeleteProvider(int id)
        {
            var provider = _context.Providers.FirstOrDefault(x => x.Id == id);
            if (provider == null)
            {
                TempData["Error"] = "Sağlayıcı bulunamadı.";
                return RedirectToAction("Index", new { section = "providers" });
            }
            _context.Providers.Remove(provider);
            _context.SaveChanges();
            return RedirectToAction("Index", new { section = "providers" });
        }

        [HttpGet("providers/edit/{id}")]
        public IActionResult EditProviderModal(int id)
        {
            var provider = _context.Providers.FirstOrDefault(x => x.Id == id);

            if (provider == null)
                return NotFound();

            var model = new ProviderViewModel
            {
                Id = provider.Id,
                Name = provider.Name,
                Alias = provider.Alias,
                ApiKey = provider.ApiKey,
                LowBalanceNotificationEnabled = provider.LowBalanceNotificationEnabled,
                BalanceLimit = provider.BalanceLimit
            };

            return PartialView("_EditProviderModal", model);
        }


        [HttpGet("providers/add")]
        public IActionResult AddProviderModal()
        {
            return PartialView("_AddProviderModal");
        }

        [HttpPost("providers/edit")]
        public async Task<IActionResult> EditProvider(int id, string apiKey, bool alias_checkbox, string alias, bool balance_checkbox, string balance_limit)
        {
            // 1. Veritabanından güncellenecek kaydı ID'ye göre bul
            var provider = await _context.Providers.FindAsync(id);

            if (provider == null)
            {
                TempData["Error"] = "Sağlayıcı bulunamadı.";
                return RedirectToAction("Index", new { section = "providers" });
            }

            provider.ApiKey = apiKey != null ? apiKey : provider.ApiKey;
            provider.AliasEnabled = alias_checkbox;
            provider.Alias = alias_checkbox ? alias : null;
            provider.LowBalanceNotificationEnabled = balance_checkbox;

            // Bakiye limitini güvenli bir şekilde parse et (çökmeleri önlemek için)
            if (balance_checkbox && decimal.TryParse(balance_limit, out decimal parsedLimit))
            {
                provider.BalanceLimit = parsedLimit;
            }
            else
            {
                provider.BalanceLimit = 0;
            }

            _context.Providers.Update(provider);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", new { section = "providers" });
        }

        [HttpPost("providers/sync")]
        public async Task<IActionResult> SyncAllProvidersBalance()
        {
            // 1. Veritabanındaki tüm sağlayıcıları çekiyoruz
            var providers = _context.Providers.ToList();

            if (!providers.Any())
            {
                TempData["Error"] = "Sistemde kayıtlı hiçbir sağlayıcı bulunamadı.";
                return RedirectToAction("Index", new { section = "providers" });
            }

            int successCount = 0;
            int errorCount = 0;

            // 2. Her bir sağlayıcı için döngü başlatıyoruz
            foreach (var provider in providers)
            {
                // KURAL 1: Sorgulama başarılı olsa da olmasa da LastCheckAt dolacak
                provider.LastCheckAt = DateTime.UtcNow;

                // API Key yoksa hiç istek atmadan diğerine geç (boşa yorulmasın)
                if (string.IsNullOrWhiteSpace(provider.ApiKey))
                {
                    errorCount++;
                    continue;
                }

                try
                {
                    // API'ye bakiye sorgusu atıyoruz
                    var response = await _smmApiService.GetBalanceAsync(provider.ApiUrl, provider.ApiKey);

                    // Eğer API bize hata mesajı dönmediyse (İşlem BAŞARILIYSA)
                    if (string.IsNullOrEmpty(response.Error))
                    {
                        // KURAL 2: Sadece başarılıysa LastSyncAt dolacak
                        provider.LastSyncAt = DateTime.UtcNow;

                        // KURAL 3: Para birimi (Currency) güncellenecek
                        if (!string.IsNullOrEmpty(response.Currency))
                        {
                            provider.Currency = response.Currency;
                        }

                        // KURAL 4: Bakiye miktarı (CurrentBalance) güncellenecek
                        // API'den string gelen "15.50" verisini güvenli bir şekilde Decimal'e çeviriyoruz
                        if (decimal.TryParse(response.Balance, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal balance))
                        {
                            provider.CurrentBalance = balance;
                        }

                        successCount++;
                    }
                    else
                    {
                        // API'den "Yanlış Key" gibi bir hata döndü
                        errorCount++;
                    }
                }
                catch
                {
                    // Sistemsel hata (Site çökmüş, internet gitmiş vb.)
                    // Döngü kırılmasın diye hatayı yakalıyoruz ama bir şey yapmıyoruz. 
                    // LastCheckAt zaten yukarıda güncellendiği için kontrol edildiği ama başarısız olduğu belli olacak.
                    errorCount++;
                }
            }

            // 3. KURAL 5: UpdatedAt dolmayacak (Koda bilerek yazmadık, EF Core dokunmayacak)
            // Tüm güncellemeleri tek seferde veritabanına kaydediyoruz
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Senkronizasyon tamamlandı! Başarılı: {successCount}, Başarısız/Eksik: {errorCount}";
            return RedirectToAction("Index", new { section = "providers" });
        }

        [HttpPost("general/update")]
        public async Task<IActionResult> UpdateGeneral(GeneralSettingsViewModel model)
        {
            try
            {
                // 1. Mevcut ayarları JSON'dan çek (Yolları korumak için)
                var currentSettings = await _settingsService.GetSettingsAsync();

                var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                // 2. Favicon
                if (model.FaviconFile != null && model.FaviconFile.Length > 0)
                {
                    var fileName = "favicon.ico";
                    var path = Path.Combine(uploadsRoot, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.FaviconFile.CopyToAsync(stream);
                    }
                    model.FaviconPath = "/" + fileName;
                }
                else
                {
                    // Yeni dosya seçilmediyse eski yolu koru
                    model.FaviconPath = currentSettings.FaviconPath;
                }

                // 3. Site Logo
                if (model.LogoFile != null && model.LogoFile.Length > 0)
                {
                    var ext = Path.GetExtension(model.LogoFile.FileName);
                    var fileName = "logo" + ext;
                    var path = Path.Combine(uploadsRoot, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.LogoFile.CopyToAsync(stream);
                    }
                    model.LogoPath = "/" + fileName;
                }
                else
                {
                    model.LogoPath = currentSettings.LogoPath;
                }

                // 4. Dar (Small) Logo
                if (model.SmallLogoFile != null && model.SmallLogoFile.Length > 0)
                {
                    var ext = Path.GetExtension(model.SmallLogoFile.FileName);
                    var fileName = "logo-small" + ext;
                    var path = Path.Combine(uploadsRoot, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.SmallLogoFile.CopyToAsync(stream);
                    }
                    model.SmallLogoPath = "/" + fileName;
                }
                else
                {
                    model.SmallLogoPath = currentSettings.SmallLogoPath;
                }

                // 5. Custom sitemap / robots dosyaları
                if (model.CustomSitemapFile != null && model.CustomSitemapFile.Length > 0)
                {
                    var fileName = "sitemap.xml";
                    var path = Path.Combine(uploadsRoot, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.CustomSitemapFile.CopyToAsync(stream);
                    }
                    model.SitemapPath = "/" + fileName;
                }
                else
                {
                    model.SitemapPath = currentSettings.SitemapPath;
                }

                if (model.CustomRobotsFile != null && model.CustomRobotsFile.Length > 0)
                {
                    var fileName = "robots.txt";
                    var path = Path.Combine(uploadsRoot, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.CustomRobotsFile.CopyToAsync(stream);
                    }
                    model.RobotsPath = "/" + fileName;
                }
                else
                {
                    model.RobotsPath = currentSettings.RobotsPath;
                }

                // 6. Dosya nesnelerini JSON'a girmemesi için temizle
                model.FaviconFile = null;
                model.LogoFile = null;
                model.SmallLogoFile = null;
                model.CustomSitemapFile = null;
                model.CustomRobotsFile = null;

                // 7. JSON'a kaydet
                await _settingsService.SaveSettingsAsync(model);

                TempData["Success"] = "Settings saved successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Settings save error");
                TempData["Error"] = "An error occurred while saving settings.";
            }

            return RedirectToAction("Index", new { section = "general" });
        }


    }
}
