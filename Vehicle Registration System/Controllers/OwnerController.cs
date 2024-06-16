using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Repositories.Implementations;
using Vehicle_Registration_System.Repositories.Interfaces;
using Vehicle_Registration_System.Services.Interfaces;

namespace Vehicle_Registration_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly IMemoryCache _memoryCache;


        public OwnerController(IOwnerService ownerService, IMemoryCache memoryCache)
        {
            _ownerService = ownerService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Owner>>> GetAllOwners()
        {
            if (_memoryCache.TryGetValue("AllOwners", out IEnumerable<Owner> owners))
            {
                return Ok(owners);
            }

            var response = await _ownerService.GetAllOwners();
            _memoryCache.Set("AllOwners", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/owner/{id}")]
        [Authorize]
        public async Task<ActionResult> GetOwnerById(int id)
        {
            if (_memoryCache.TryGetValue($"Owner_{id}", out Owner owner))
            {
                return Ok(owner);
            }

            var response = await _ownerService.GetOwnerById(id);
            _memoryCache.Set($"Owner_{id}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddOwner(InputOwner request)
        {
            await _ownerService.AddOwner(request);

            return Ok();
        }

        [HttpDelete("/owner/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteOwner(int id)
        {
            await _ownerService.DeleteOwner(id);
            return Ok();
        }

        [HttpPut("/owner/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateOwner (EditOwner editOwner, int id)
        {
            await _ownerService.UpdateOwner(editOwner, id);
            return Ok();
        }

        [HttpGet("/findByCity/{placeOfBirth}")]
        [Authorize]
        public async Task<ActionResult> FindByCity(string placeOfBirth)
        {
            if (_memoryCache.TryGetValue($"OwnersByCity_{placeOfBirth}", out IEnumerable<Owner> owners))
            {
                return Ok(owners);
            }

            var response = await _ownerService.FindByCity(placeOfBirth);
            _memoryCache.Set($"OwnersByCity_{placeOfBirth}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/findByVehicle/{manufacturer}/{model}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Owner>>> FindOwnerByVehicle(string manufacturer, string model)
        {
            if (_memoryCache.TryGetValue($"OwnersByVehicle_{manufacturer}_{model}", out IEnumerable<Owner> owners))
            {
                return Ok(owners);
            }

            var response = await _ownerService.FindOwnerByVehicle(manufacturer, model);
            _memoryCache.Set($"OwnersByVehicle_{manufacturer}_{model}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/licensesByCity/{placeOfBirth}")]
        [Authorize]
        public async Task<ActionResult<float>> GetLicensesByCity(string placeOfBirth)
        {
            if (_memoryCache.TryGetValue($"LicensesByCity_{placeOfBirth}", out float count))
            {
                return Ok(count);
            }

            var response = await _ownerService.GetLicensesByCity(placeOfBirth);
            _memoryCache.Set($"LicensesByCity_{placeOfBirth}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }
    }
}
