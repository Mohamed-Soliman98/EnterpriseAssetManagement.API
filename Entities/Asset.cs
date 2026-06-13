namespace EnterpriseAssetManagement.API.Entities
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AssetType { get; set; } = string.Empty; 
        public string Model { get; set; } = string.Empty;     
        public string SerialNumber { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public bool IsAssigned { get; set; } = false; 

    }
}
