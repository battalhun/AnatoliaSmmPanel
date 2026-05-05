using System.ComponentModel.DataAnnotations;

namespace AnatoliaSmmPanel.Areas.Admin.Enums
{
    public enum TicketSystemType
    {
        [Display(Name = "Enabled for all users")]
        EnabledForAll = 1,

        [Display(Name = "Enabled for paying users")]
        EnabledForPaying = 2,

        [Display(Name = "Disabled")]
        Disabled = 3
    }
}