namespace EnterpriseAssetManagement.API.DTOs
{
    public class AssetUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string AssetType { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
        public string? MacAddress { get; set; }
    }
}
