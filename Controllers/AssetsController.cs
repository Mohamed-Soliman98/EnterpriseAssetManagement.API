using EnterpriseAssetManagement.API.DTOs;
using EnterpriseAssetManagement.API.Entities;
using EnterpriseAssetManagement.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseAssetManagement.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IGenericRepository<Asset> _assetRepo;

        public AssetsController(IGenericRepository<Asset> assetRepo)
        {
            _assetRepo = assetRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _assetRepo.GetAllAsync();
            return Ok(assets);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _assetRepo.GetByIdAsync(id);
            if (asset == null) return NotFound(new { message = $"الجهاز رقم {id} مش موجود في السيستم" });

            return Ok(asset);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,IT_Manager")]
        public async Task<IActionResult> Create(Asset asset)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _assetRepo.AddAsync(asset);
            await _assetRepo.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , AssetUpdateDto assetUpdateDto)
        {
            var assetfromDb = await _assetRepo.GetByIdAsync(id);
            if (assetfromDb == null)
            {
                return NotFound(new { message = $"الجهاز رقم {id} مش موجود في السيستم عشان تعدله" });
            }
            assetfromDb.Name = assetUpdateDto.Name;
            assetfromDb.AssetType = assetUpdateDto.AssetType;
            assetfromDb.SerialNumber = assetUpdateDto.SerialNumber;
            assetfromDb.IPAddress = assetUpdateDto.IpAddress;
            assetfromDb.MacAddress = assetUpdateDto.MacAddress;
             

            _assetRepo.Update(assetfromDb);
            await _assetRepo.SaveChangesAsync();
            return Ok(new { message = "تم نعديل الجهاز بنجاح من قاعدة البيانات" , data = assetfromDb });
        }
          
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _assetRepo.GetByIdAsync(id);
            if (asset == null)
            {
                return NotFound(new { message = $"الجهاز رقم {id} مش موجود في السيستم عشان أمسحه" });
            }

            _assetRepo.Delete(asset);

            await _assetRepo.SaveChangesAsync();

            return Ok(new { message = "تم حذف الجهاز بنجاح من قاعدة البيانات" });
        }
     }
    }
