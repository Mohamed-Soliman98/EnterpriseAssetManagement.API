namespace EnterpriseAssetManagement.API.DTOs
{
    public class GetMaintenanceLogDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime MaintenanceDate { get; set; }
        public decimal Cost { get; set; }
        public int AssetId { get; set; }
        public string AssetName { get; set; } = string.Empty; 
        public string EngineerName { get; set; } = string.Empty; 
    }
}
