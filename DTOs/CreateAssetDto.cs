using System.ComponentModel.DataAnnotations;

namespace EnterpriseAssetManagement.API.DTOs
{
    public class CreateAssetDto
    {
        [Required(ErrorMessage = "Device name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Asset type is required")]
        public string AssetType { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
        public string? MacAddress { get; set; }
    }
}