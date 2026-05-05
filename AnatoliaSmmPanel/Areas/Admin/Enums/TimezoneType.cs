using System.ComponentModel.DataAnnotations;

namespace AnatoliaSmmPanel.Areas.Admin.Enums
{
    public enum TimezoneType
    {
        [Display(Name = "(UTC -12:00) International Date Line West")]
        UTC_Minus12 = -12,

        [Display(Name = "(UTC -11:00) Coordinated Universal Time-11, Samoa")]
        UTC_Minus11 = -11,

        [Display(Name = "(UTC -10:00) Hawaii")]
        UTC_Minus10 = -10,

        [Display(Name = "(UTC -09:00) Alaska")]
        UTC_Minus09 = -9,

        [Display(Name = "(UTC -08:00) Pacific Time (US & Canada)")]
        UTC_Minus08 = -8,

        [Display(Name = "(UTC -07:00) Mountain Time (US & Canada)")]
        UTC_Minus07 = -7,

        [Display(Name = "(UTC -06:00) Central Time (US & Canada), Mexico City")]
        UTC_Minus06 = -6,

        [Display(Name = "(UTC -05:00) Eastern Time (US & Canada), Bogota, Lima")]
        UTC_Minus05 = -5,

        [Display(Name = "(UTC -04:00) Atlantic Time (Canada), Caracas, La Paz")]
        UTC_Minus04 = -4,

        [Display(Name = "(UTC -03:30) Newfoundland")]
        UTC_Minus03_30 = -35, // Buçuklu saatler için özel isimlendirme

        [Display(Name = "(UTC -03:00) Buenos Aires, Brasilia, Greenland")]
        UTC_Minus03 = -3,

        [Display(Name = "(UTC -02:00) Mid-Atlantic, Coordinated Universal Time-02")]
        UTC_Minus02 = -2,

        [Display(Name = "(UTC -01:00) Azores, Cape Verde Is.")]
        UTC_Minus01 = -1,

        [Display(Name = "(UTC +00:00) Universal Time Coordinated, London, Dublin")]
        UTC_00 = 0,

        [Display(Name = "(UTC +01:00) Amsterdam, Berlin, Paris, Rome")]
        UTC_Plus01 = 1,

        [Display(Name = "(UTC +02:00) Athens, Cairo, Jerusalem, Kyiv")]
        UTC_Plus02 = 2,

        [Display(Name = "(UTC +03:00) Istanbul, Moscow, Riyadh, Baghdad")]
        UTC_Plus03 = 3,

        [Display(Name = "(UTC +03:30) Tehran")]
        UTC_Plus03_30 = 35,

        [Display(Name = "(UTC +04:00) Abu Dhabi, Muscat, Baku")]
        UTC_Plus04 = 4,

        [Display(Name = "(UTC +04:30) Kabul")]
        UTC_Plus04_30 = 45,

        [Display(Name = "(UTC +05:00) Islamabad, Karachi, Tashkent")]
        UTC_Plus05 = 5,

        [Display(Name = "(UTC +05:30) Chennai, Kolkata, Mumbai, New Delhi")]
        UTC_Plus05_30 = 55,

        [Display(Name = "(UTC +05:45) Kathmandu")]
        UTC_Plus05_45 = 545,

        [Display(Name = "(UTC +06:00) Astana, Dhaka, Almaty")]
        UTC_Plus06 = 6,

        [Display(Name = "(UTC +06:30) Yangon (Rangoon)")]
        UTC_Plus06_30 = 65,

        [Display(Name = "(UTC +07:00) Bangkok, Hanoi, Jakarta")]
        UTC_Plus07 = 7,

        [Display(Name = "(UTC +08:00) Beijing, Singapore, Perth, Taipei")]
        UTC_Plus08 = 8,

        [Display(Name = "(UTC +09:00) Tokyo, Seoul, Osaka, Sapporo")]
        UTC_Plus09 = 9,

        [Display(Name = "(UTC +09:30) Adelaide, Darwin")]
        UTC_Plus09_30 = 95,

        [Display(Name = "(UTC +10:00) Brisbane, Sydney, Melbourne, Guam")]
        UTC_Plus10 = 10,

        [Display(Name = "(UTC +11:00) Solomon Is., New Caledonia")]
        UTC_Plus11 = 11,

        [Display(Name = "(UTC +12:00) Auckland, Wellington, Fiji")]
        UTC_Plus12 = 12,

        [Display(Name = "(UTC +13:00) Nuku'alofa, Samoa")]
        UTC_Plus13 = 13,

        [Display(Name = "(UTC +14:00) Kiritimati Island")]
        UTC_Plus14 = 14
    }
}