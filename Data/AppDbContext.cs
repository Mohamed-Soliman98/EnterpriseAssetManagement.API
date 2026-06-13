using EnterpriseAssetManagement.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseAssetManagement.API.Data
{
    public class AppDbContext :IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Asset> Assets { get; set; }
        public DbSet<MaintenanceLog> maintenanceLogs { get; set; }
    }
}
