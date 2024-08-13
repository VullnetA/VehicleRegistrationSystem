using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Services.Interfaces;

namespace Vehicle_Registration_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMemoryCache _memoryCache;

        public VehicleController(IVehicleService vehicleService, IMemoryCache memoryCache)
        {
            _vehicleService = vehicleService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAllVehicles()
        {
            if (_memoryCache.TryGetValue("AllVehicles", out IEnumerable<VehicleDto> vehicles))
            {
                return Ok(vehicles);
            }

            var response = await _vehicleService.GetAllVehicles();
            if (response == null) return NotFound();
            _memoryCache.Set("AllVehicles", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/vehicle/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<VehicleDto>> GetVehicleById(int id)
        {
            if (_memoryCache.TryGetValue($"Vehicle_{id}", out VehicleDto vehicle))
            {
                return Ok(vehicle);
            }

            var response = await _vehicleService.GetVehicleById(id);
            if (response == null) return NotFound();
            _memoryCache.Set($"Vehicle_{id}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddVehicle(RegisterVehicle register)
        {
            await _vehicleService.AddVehicle(register);
            _memoryCache.Remove("AllVehicles");
            _memoryCache.Remove("CountUnregistered");
            _memoryCache.Remove("CountRegistered");
            _memoryCache.Remove($"CountByBrand_{register.Manufacturer}");
            _memoryCache.Remove($"CountTransmission_{register.Transmission}");
            _memoryCache.Remove($"CountByFuelType_{register.Fuel}");
            _memoryCache.Remove($"CountByYear_{register.Year}");
            _memoryCache.Remove($"CountByCategory_{register.Category}");

            return Ok();
        }

        [HttpDelete("/vehicle/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            await _vehicleService.DeleteVehicle(id);
            _memoryCache.Remove("AllVehicles");
            _memoryCache.Remove($"Vehicle_{id}");
            return Ok();
        }

        [HttpPut("/vehicle/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateVehicle(EditVehicle editVehicle, int id)
        {
            await _vehicleService.UpdateVehicle(editVehicle, id);
            _memoryCache.Remove("AllVehicles");
            _memoryCache.Remove($"Vehicle_{id}");
            return Ok();
        }

        [HttpGet("/vehiclesbyowner/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehiclesByOwner(int id)
        {
            if (_memoryCache.TryGetValue($"VehiclesByOwner_{id}", out IEnumerable<VehicleDto> vehicles))
            {
                return Ok(vehicles);
            }

            var response = await _vehicleService.FindAllByOwner(id);
            if (response == null) return NotFound();
            _memoryCache.Set($"VehiclesByOwner_{id}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/vehiclesbyyear/{year}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehiclesByYear(int year)
        {
            if (_memoryCache.TryGetValue($"VehiclesByYear_{year}", out IEnumerable<VehicleDto> vehicles))
            {
                return Ok(vehicles);
            }

            var response = await _vehicleService.FindByYear(year);
            if (response == null) return NotFound();
            _memoryCache.Set($"VehiclesByYear_{year}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/vehiclespower/{power}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehiclesWithMorePower(int power)
        {
            if (_memoryCache.TryGetValue($"VehiclesWithMorePower_{power}", out IEnumerable<VehicleDto> vehicles))
            {
                return Ok(vehicles);
            }

            var response = await _vehicleService.FindByHorsepower(power);
            if (response == null) return NotFound();
            _memoryCache.Set($"VehiclesWithMorePower_{power}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/vehiclesbyfuel/{fuel}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehiclesByFuel(string fuel)
        {
            if (_memoryCache.TryGetValue($"VehiclesByFuel_{fuel}", out IEnumerable<VehicleDto> vehicles))
            {
                return Ok(vehicles);
            }

            var response = await _vehicleService.FindByFuelType(fuel);
            if (response == null) return NotFound();
            _memoryCache.Set($"VehiclesByFuel_{fuel}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/vehiclesbybrand/{brand}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehiclesByBrand(string brand)
        {
            if (_memoryCache.TryGetValue($"VehiclesByBrand_{brand}", out IEnumerable<VehicleDto> vehicles))
            {
                return Ok(vehicles);
            }

            var response = await _vehicleService.FindByBrand(brand);
            if (response == null) return NotFound();
            _memoryCache.Set($"VehiclesByBrand_{brand}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/countbybrand/{brand}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<long>> CountVehiclesByBrand(string brand)
        {
            if (_memoryCache.TryGetValue($"CountByBrand_{brand}", out long count))
            {
                return Ok(count);
            }

            var response = await _vehicleService.CountByBrand(brand);
            _memoryCache.Set($"CountByBrand_{brand}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/countbyfueltype/{fuel}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<long>> CountVehiclesByFuelType(string fuel)
        {
            if (_memoryCache.TryGetValue($"CountByFuelType_{fuel}", out long count))
            {
                return Ok(count);
            }

            var response = await _vehicleService.CountByFuelType(fuel);
            _memoryCache.Set($"CountByFuelType_{fuel}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/countbyyear/{year}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<long>> CountVehiclesByYear(int year)
        {
            if (_memoryCache.TryGetValue($"CountByYear_{year}", out long count))
            {
                return Ok(count);
            }

            var response = await _vehicleService.CountByYear(year);
            _memoryCache.Set($"CountByYear_{year}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/countbycategory/{category}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<long>> CountVehiclesByCategory(string category)
        {
            if (_memoryCache.TryGetValue($"CountByCategory_{category}", out long count))
            {
                return Ok(count);
            }

            var response = await _vehicleService.CountByCategory(category);
            _memoryCache.Set($"CountByCategory_{category}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/countunregistered")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<long>> CountUnregisteredVehicles()
        {
            if (_memoryCache.TryGetValue("CountUnregistered", out long count))
            {
                return Ok(count);
            }

            var response = await _vehicleService.CountUnregistered();
            _memoryCache.Set("CountUnregistered", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/countregistered")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<long>> CountRegisteredVehicles()
        {
            if (_memoryCache.TryGetValue("CountRegistered", out long count))
            {
                return Ok(count);
            }

            var response = await _vehicleService.CountRegistered();
            _memoryCache.Set("CountRegistered", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/counttransmission/{transmission}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<long>> CountTransmission(string transmission)
        {
            if (_memoryCache.TryGetValue($"CountTransmission_{transmission}", out long count))
            {
                return Ok(count);
            }

            var response = await _vehicleService.CountTransmission(transmission);
            _memoryCache.Set($"CountTransmission_{transmission}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/licenseplate/{licensePlate}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<VehicleDto>> FindVehicleByLicensePlate(string licensePlate)
        {
            if (_memoryCache.TryGetValue($"VehicleByLicensePlate_{licensePlate}", out VehicleDto vehicle))
            {
                return Ok(vehicle);
            }

            var response = await _vehicleService.FindByLicensePlate(licensePlate);
            if (response == null) return NotFound();
            _memoryCache.Set($"VehicleByLicensePlate_{licensePlate}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/checkregistration/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> CheckVehicleRegistration(int id)
        {
            if (_memoryCache.TryGetValue($"CheckRegistration_{id}", out bool isRegistered))
            {
                return Ok(isRegistered);
            }

            var response = await _vehicleService.CheckRegistration(id);
            _memoryCache.Set($"CheckRegistration_{id}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }
    }
}
