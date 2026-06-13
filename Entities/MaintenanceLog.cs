using EnterpriseAssetManagement.API.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseAssetManagement.API.Entities
{
    public class MaintenanceLog
    {
        public int Id {  get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public DateTime MaintenanceDate { get; set; }

        [ForeignKey("Asset")]
        public int AssetId { get; set; }
        public Asset? Asset { get; set; }

        [ForeignKey("PerformedBy")]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
    }
}
