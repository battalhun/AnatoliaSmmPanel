using System.ComponentModel.DataAnnotations;

namespace AnatoliaSmmPanel.Areas.Admin.Enums
{
    public enum SignupPageStatus
    {
        [Display(Name = "Enabled")]
        Enabled = 1,

        [Display(Name = "Disabled")]
        Disabled = 2
    }
}