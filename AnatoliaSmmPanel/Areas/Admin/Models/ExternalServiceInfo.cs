using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AnatoliaSmmPanel.Areas.Admin.Models
{
    public class ExternalServiceInfo
    {
        [Key]
        public int Id { get; set; }

        public string ExternalServiceId { get; set; }

        [MaxLength(250)]
        public string ExternalName { get; set; }

        [MaxLength(50)]
        public string ExternalType { get; set; }


        [Column(TypeName = "decimal(18,4)")]
        public decimal ExternalRate { get; set; }

        public int ExternalMin { get; set; }
        public int ExternalMax { get; set; }

        public string ExternalCategoryName { get; set; }

        public DateTime? LastSyncAt { get; set; }
    }
}
