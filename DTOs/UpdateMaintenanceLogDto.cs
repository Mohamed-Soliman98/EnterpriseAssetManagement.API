using System.ComponentModel.DataAnnotations;

namespace EnterpriseAssetManagement.API.DTOs
{
    public class UpdateMaintenanceLogDto
    {
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Cost must be a positive value")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Asset ID is required")]
        public int AssetId { get; set; }
    }
}
