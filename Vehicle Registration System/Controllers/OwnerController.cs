using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Services.Interfaces;

namespace Vehicle_Registration_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly IVehicleService _vehicleService;
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public OwnerController(IOwnerService ownerService, IVehicleService vehicleService, IMemoryCache memoryCache, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _ownerService = ownerService;
            _vehicleService = vehicleService;
            _memoryCache = memoryCache;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetAllOwners()
        {
            if (_memoryCache.TryGetValue("AllOwners", out IEnumerable<OwnerDto> owners))
            {
                return Ok(owners);
            }

            var response = await _ownerService.GetAllOwners();
            if (response == null) return NotFound();
            _memoryCache.Set("AllOwners", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/owner/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OwnerDto>> GetOwnerById(int id)
        {
            if (_memoryCache.TryGetValue($"Owner_{id}", out OwnerDto owner))
            {
                return Ok(owner);
            }

            var response = await _ownerService.GetOwnerById(id);
            if (response == null) return NotFound();
            _memoryCache.Set($"Owner_{id}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddOwner(InputOwner request)
        {
            var owner = await _ownerService.AddOwner(request);

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                OwnerId = owner.Id
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!await _roleManager.RoleExistsAsync("Owner"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Owner"));
            }
            await _userManager.AddToRoleAsync(user, "Owner");

            return Ok(new { OwnerId = owner.Id });
        }

        [HttpDelete("/owner/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteOwner(int id)
        {
            await _ownerService.DeleteOwner(id);
            _memoryCache.Remove($"Owner_{id}");
            _memoryCache.Remove("AllOwners");
            return Ok();
        }

        [HttpPut("/owner/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateOwner(EditOwner editOwner, int id)
        {
            await _ownerService.UpdateOwner(editOwner, id);
            _memoryCache.Remove($"Owner_{id}");
            _memoryCache.Remove("AllOwners");
            return Ok();
        }

        [HttpGet("/findByCity/{placeOfBirth}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> FindByCity(string placeOfBirth)
        {
            if (_memoryCache.TryGetValue($"OwnersByCity_{placeOfBirth}", out IEnumerable<OwnerDto> owners))
            {
                return Ok(owners);
            }

            var response = await _ownerService.FindByCity(placeOfBirth);
            if (response == null) return NotFound();
            _memoryCache.Set($"OwnersByCity_{placeOfBirth}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/findByVehicle/{manufacturer}/{model}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> FindOwnerByVehicle(string manufacturer, string model)
        {
            if (_memoryCache.TryGetValue($"OwnersByVehicle_{manufacturer}_{model}", out IEnumerable<OwnerDto> owners))
            {
                return Ok(owners);
            }

            var response = await _ownerService.FindOwnerByVehicle(manufacturer, model);
            if (response == null) return NotFound();
            _memoryCache.Set($"OwnersByVehicle_{manufacturer}_{model}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/licensesByCity/{placeOfBirth}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<long>> GetLicensesByCity(string placeOfBirth)
        {
            if (_memoryCache.TryGetValue($"LicensesByCity_{placeOfBirth}", out long count))
            {
                return Ok(count);
            }

            var response = await _ownerService.GetLicensesByCity(placeOfBirth);
            _memoryCache.Set($"LicensesByCity_{placeOfBirth}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/searchByName/{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetOwnersByName(string name)
        {
            if (_memoryCache.TryGetValue($"OwnersByName_{name}", out IEnumerable<OwnerDto> owners))
            {
                return Ok(owners);
            }

            var response = await _ownerService.GetOwnersByName(name);
            if (response == null) return NotFound();
            _memoryCache.Set($"OwnersByName_{name}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }
    }
}
