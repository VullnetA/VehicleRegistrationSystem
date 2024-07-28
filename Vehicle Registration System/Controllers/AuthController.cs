using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Vehicle_Registration_System.Data;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Services.AuthenticationService;

namespace Vehicle_Registration_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            TokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegistrationRequestDTO request)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);

            if (userExists != null)
            {
                return BadRequest("User already exists");
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User created successfully");
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Authenticate([FromBody] AuthRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request body is empty");
            }

            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser == null)
            {
                return BadRequest("No user found");
            }

            if (await _userManager.IsLockedOutAsync(managedUser))
            {
                return BadRequest("User account locked out.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
            {
                await _userManager.AccessFailedAsync(managedUser);

                if (await _userManager.IsLockedOutAsync(managedUser))
                {
                    return BadRequest("User account locked out.");
                }

                return BadRequest("Invalid login attempt.");
            }

            await _userManager.ResetAccessFailedCountAsync(managedUser);

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email) as ApplicationUser;
            if (userInDb is null)
            {
                return Unauthorized();
            }

            var accessToken = await _tokenService.CreateToken(userInDb);
            var roles = await _userManager.GetRolesAsync(userInDb);
            await _context.SaveChangesAsync();

            var response = new AuthResponseDTO
            {
                UserId = userInDb.Id,
                Username = userInDb.UserName,
                Email = userInDb.Email,
                Token = accessToken,
                Roles = roles.ToList(),
                OwnerId = userInDb.OwnerId
            };

            Console.WriteLine("Authentication successful. Returning response: " + JsonConvert.SerializeObject(response));

            return Ok(response);
        }

        [HttpPost("role")]
        public async Task<ActionResult> CreateRoles(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            return Ok();
        }

        [HttpPost("assign")]
        public async Task<ActionResult> AssignRoleToUser(string username, string roleName)
        {
            var user = await _userManager.FindByNameAsync(username)
                ?? throw new ApplicationException($"User with username '{username}' not found.");
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                throw new ApplicationException($"Role '{roleName}' does not exist.");
            }

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
            return Ok();
        }
    }
}
