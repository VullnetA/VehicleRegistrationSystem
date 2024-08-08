using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Services.Interfaces;

namespace Vehicle_Registration_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceController : ControllerBase
    {
        private readonly IInsuranceService _insuranceService;
        private readonly IMemoryCache _memoryCache;

        public InsuranceController(IInsuranceService insuranceService, IMemoryCache memoryCache)
        {
            _insuranceService = insuranceService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Insurance>>> GetAllInsurances()
        {
            if (_memoryCache.TryGetValue("AllInsurances", out IEnumerable<Insurance> insurances))
            {
                return Ok(insurances);
            }

            var response = await _insuranceService.GetAllInsurances();
            if (response == null) return NotFound();
            _memoryCache.Set("AllInsurances", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/insurance/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Insurance>> GetInsuranceById(int id)
        {
            if (_memoryCache.TryGetValue($"Insurance_{id}", out Insurance insurance))
            {
                return Ok(insurance);
            }

            var response = await _insuranceService.FindInsuranceById(id);
            if (response == null) return NotFound();
            _memoryCache.Set($"Insurance_{id}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/insurance/vehicle/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Insurance>> GetInsuranceByVehicleId(int id)
        {
            if (_memoryCache.TryGetValue($"InsuranceVehicle_{id}", out Insurance insurance))
            {
                return Ok(insurance);
            }

            var response = await _insuranceService.FindInsuranceByVehicleId(id);
            if (response == null) return NotFound();
            _memoryCache.Set($"InsuranceVehicle_{id}", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddInsurance(MakeInsurance make)
        {
            await _insuranceService.AddInsurance(make);
            _memoryCache.Remove("AllInsurances");
            return Ok();
        }

        [HttpDelete("/insurance/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteInsurance(int id)
        {
            await _insuranceService.DeleteInsurance(id);
            _memoryCache.Remove($"Insurance_{id}");
            _memoryCache.Remove("AllInsurances");
            return Ok();
        }

        [HttpGet("/count")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<long>> CountInsurances()
        {
            if (_memoryCache.TryGetValue("CountInsurances", out long count))
            {
                return Ok(count);
            }

            var response = await _insuranceService.CountInsurance();
            _memoryCache.Set("CountInsurances", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("/expiredinsurances")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> FindExpiredInsurances()
        {
            if (_memoryCache.TryGetValue("ExpiredInsurances", out IEnumerable<Vehicle> expiredInsurances))
            {
                return Ok(expiredInsurances);
            }

            var response = await _insuranceService.FindExpiredInsurance();
            if (response == null) return NotFound();
            _memoryCache.Set("ExpiredInsurances", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpPut("/insurance/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateInsurance(EditInsurance edit, int id)
        {
            await _insuranceService.UpdateInsurance(edit, id);
            _memoryCache.Remove($"Insurance_{id}");
            _memoryCache.Remove("AllInsurances");
            return Ok();
        }
    }
}
