using System.ComponentModel.DataAnnotations;

namespace AnatoliaSmmPanel.Areas.Admin.Enums
{
    public enum CurrencyFormatType
    {
        [Display(Name = "1000.00")]
        Format_1000_Dot_00 = 1,

        [Display(Name = "1000,00")]
        Format_1000_Comma_00 = 2,

        [Display(Name = "1,000.12")]
        Format_1_Comma_000_Dot_12 = 3,

        [Display(Name = "1.000,12")]
        Format_1_Dot_000_Comma_12 = 4
    }
}