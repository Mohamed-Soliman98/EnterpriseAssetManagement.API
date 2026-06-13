using EnterpriseAssetManagement.API.Data;
using EnterpriseAssetManagement.API.DTOs;
using EnterpriseAssetManagement.API.Entities;
using EnterpriseAssetManagement.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnterpriseAssetManagement.API.Controllers
{
    [Authorize(Roles = "Admin,IT_Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceLogsController : ControllerBase
    {
        private readonly IGenericRepository<MaintenanceLog> _maintenanceLog;

        public MaintenanceLogsController(IGenericRepository<MaintenanceLog> maintenanceLog)
        {
            _maintenanceLog = maintenanceLog;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices]AppDbContext context)
        {
            var logs = await context.maintenanceLogs
                              .Include(l => l.Asset)
                              .Include(l => l.User)
                              .ToListAsync();

            var logDtos = logs.Select(log => new GetMaintenanceLogDto
            {
                Id = log.Id,
                Description = log.Description,
                MaintenanceDate = log.MaintenanceDate,
                Cost = log.Cost,
                AssetId = log.AssetId,
                AssetName = log.Asset?.Name ?? "جهاز ممسوح",
                EngineerName = log.User?.FullName ?? "مهندس غير معروف"
            }).ToList();

            return Ok(logDtos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromServices] AppDbContext context)
        {

            var log = await context.maintenanceLogs
                .Include(x => x.Asset)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (log == null)
            {
                return NotFound(new { message = $"فاتورة الصيانة رقم {id} مش موجودة في السيستم" });
            }
            var dto = new GetMaintenanceLogDto()
            {
                Id =log.Id,
                Description =log.Description,
                Cost =log.Cost,
                AssetId =log.AssetId,
                AssetName =log.Asset?.Name?? "جهاز ممسوح",
                EngineerName =log.User?.FullName ?? "مهندس غير معروف",
                MaintenanceDate=log.MaintenanceDate
            };
            return Ok(dto);
        }
       
        [HttpPost]
        public async Task<IActionResult> Create(CreateMaintenanceLogDto Dtolog, [FromServices] AppDbContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            var asset = await context.Assets.FindAsync(Dtolog.AssetId);
            if (asset == null)
            {
                return NotFound($"عفواً، لا يوجد جهاز مسجل في السيستم بالرقم ({Dtolog.AssetId})");
            }
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserId == null) return Unauthorized("اليوزر غير معروف");
            var log = new MaintenanceLog()
            {
                Description = Dtolog.Description,
                Cost = Dtolog.Cost,
                AssetId = Dtolog.AssetId,
                UserId = UserId,
                MaintenanceDate =DateTime.Now,
            };
            await  _maintenanceLog.AddAsync(log);
            await  _maintenanceLog.SaveChangesAsync();

            return Ok(new
            {
                message = "تم تسجيل عملية الصيانة بنجاح في قاعدة البيانات",
                maintenanceLogId = log.Id 
            });

        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var log = await _maintenanceLog.GetByIdAsync(id);
            if (log == null)
            {
                return NotFound(new { message = $"فاتورة الصيانة رقم {id} مش موجودة في السيستم" });
            }
            _maintenanceLog.Delete(log);
            await _maintenanceLog.SaveChangesAsync();
            return Ok(new { Massage = "تم حذف سجل الصيانة بنجاح من السيستم" });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, UpdateMaintenanceLogDto logDtofromRequist, [FromServices] AppDbContext context)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var logDB = await _maintenanceLog.GetByIdAsync(id);
            if (logDB == null)
                return NotFound(new { Massage = $"فاتورة الصيانة رقم {id} مش موجودة في السيستم" });

            var asset =await context.Assets.FindAsync(logDtofromRequist.AssetId);
            if (asset == null)
            {
                return NotFound($"عفواً، لا يوجد جهاز مسجل في السيستم بالرقم ({logDtofromRequist.AssetId}) عشان نربط الصيانة بيه!");
            }
            logDB.Description = logDtofromRequist.Description;
            logDB.Cost = logDtofromRequist.Cost;
            logDB.AssetId = logDtofromRequist.AssetId;
            logDB.MaintenanceDate = DateTime.Now;
            _maintenanceLog.Update(logDB);
            await _maintenanceLog.SaveChangesAsync();
            return Ok(new { Massage = "\"تم تحديث بيانات السجل بنجاح في قاعدة البيانات\"" });
            
        }
    }
}
