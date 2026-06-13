using EnterpriseAssetManagement.API.Entities;
using Microsoft.AspNetCore.Identity;

namespace EnterpriseAssetManagement.API.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;

        public ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
    }
}
