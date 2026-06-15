using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AnatoliaSmmPanel.Areas.Admin.Enums;

namespace AnatoliaSmmPanel.Areas.Admin.ViewModels
{
    public class GeneralSettingsViewModel
    {
        [JsonIgnore]
        public IFormFile? FaviconFile { get; set; }
        public string? FaviconPath { get; set; }

        [JsonIgnore]
        public IFormFile? LogoFile { get; set; }
        public string? LogoPath { get; set; }

        [JsonIgnore]
        public IFormFile? SmallLogoFile { get; set; }
        public string? SmallLogoPath { get; set; }

        [JsonIgnore]
        public IFormFile? CustomSitemapFile { get; set; }
        public string? SitemapPath { get; set; } = "/sitemap.xml";

        [JsonIgnore]
        public IFormFile? CustomRobotsFile { get; set; }
        public string? RobotsPath { get; set; } = "/robots.txt";

        // --- AÇILIR MENÜLER (ENUMS) ---
        [Display(Name = "Timezone")]
        public TimezoneType Timezone { get; set; } = TimezoneType.UTC_00;

        [Display(Name = "Currency format")]
        public CurrencyFormatType CurrencyFormat { get; set; } = CurrencyFormatType.Format_1000_Dot_00;

        [Display(Name = "Ticket system")]
        public TicketSystemType TicketSystem { get; set; } = TicketSystemType.EnabledForAll;

        [Display(Name = "Signup page")]
        public SignupPageStatus SignupPage { get; set; } = SignupPageStatus.Enabled;

        // --- METİN ALANLARI ---
        [Display(Name = "Google verification code")]
        public string? GoogleVerificationCode { get; set; }

        [Display(Name = "Custom header code")]
        public string? CustomHeader { get; set; }

        [Display(Name = "Custom footer code")]
        public string? CustomFooter { get; set; }
    }
}