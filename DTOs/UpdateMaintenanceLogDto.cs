namespace EnterpriseAssetManagement.API.DTOs
{
    public class UpdateMaintenanceLogDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public int AssetId { get; set; }
    }
}
