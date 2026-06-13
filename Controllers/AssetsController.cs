using EnterpriseAssetManagement.API.DTOs;
using EnterpriseAssetManagement.API.Entities;
using EnterpriseAssetManagement.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin,IT_Manager")]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _assetRepo.GetAllAsync();
            return Ok(assets);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,IT_Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _assetRepo.GetByIdAsync(id);
            if (asset == null) return NotFound(new { message =$"Device number {id} not found in the system"});

            return Ok(asset);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,IT_Manager")]
        public async Task<IActionResult> Create(CreateAssetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var asset = new Asset
            {
                Name = dto.Name,
                AssetType = dto.AssetType,
                Model = dto.Model,
                SerialNumber = dto.SerialNumber,
                IPAddress = dto.IpAddress ?? string.Empty,
                MacAddress = dto.MacAddress ?? string.Empty,
                PurchaseDate = DateTime.Now
            };

            await _assetRepo.AddAsync(asset);
            await _assetRepo.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,IT_Manager")]
        public async Task<IActionResult> Update(int id , [FromBody] AssetUpdateDto assetUpdateDto)
        {
            var assetfromDb = await _assetRepo.GetByIdAsync(id);
            if (assetfromDb == null)
            {
                return NotFound(new { message = $"Device number {id} not found in the system to edit it"});
            }
            assetfromDb.Name = assetUpdateDto.Name;
            assetfromDb.AssetType = assetUpdateDto.AssetType;
            assetfromDb.Model = assetUpdateDto.Model;
            assetfromDb.SerialNumber = assetUpdateDto.SerialNumber;
            assetfromDb.IPAddress = assetUpdateDto.IpAddress ?? string.Empty;
            assetfromDb.MacAddress = assetUpdateDto.MacAddress ?? string.Empty;
             

            _assetRepo.Update(assetfromDb);
            await _assetRepo.SaveChangesAsync();
            return Ok(new { message = "Device updated successfully in the database", data = assetfromDb });
        }
          
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _assetRepo.GetByIdAsync(id);
            if (asset == null)
            {
                return NotFound(new { message = $"Device number {id} not found in the system to delete it" });
            }

            _assetRepo.Delete(asset);

            await _assetRepo.SaveChangesAsync();

            return Ok(new { message = "Device deleted successfully from the database" });
        }
     }
    }
