using EnterpriseAssetManagement.API.Data;
using EnterpriseAssetManagement.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EnterpriseAssetManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ApplicationUser user = new ApplicationUser();
            user.Email = registerDto.Email;
            user.FullName = registerDto.FullName;
            user.UserName = registerDto.Username;
            user.Department = registerDto.Department;

            IdentityResult result = await userManager.CreateAsync(user,registerDto.Password);
            if (result.Succeeded)
            {
                return Ok(new { message = "Registered successfully in the system!" });
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(" ", error.Description);
            }

            return BadRequest(ModelState);


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await userManager.FindByNameAsync(dto.Username);

            if (user != null && await userManager.CheckPasswordAsync(user, dto.Password))
            {
                //1
                var UserClaims = new List<Claim>();
                UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                UserClaims.Add(new Claim(ClaimTypes.Name,user.UserName!));

                //2
                var UserRole = await userManager.GetRolesAsync(user);
                foreach (var rolename in UserRole)
                {
                    UserClaims.Add(new Claim(ClaimTypes.Role, rolename));
                }

                //3
                var SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
                var signingCred = new SigningCredentials (SigningKey,SecurityAlgorithms.HmacSha256);

                //4
                var tokenExpiration = DateTime.UtcNow.AddHours(1);
                var Token = new JwtSecurityToken
                (
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    expires: tokenExpiration,
                    claims: UserClaims,
                    signingCredentials: signingCred
                );

                //5
                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(Token),
                    expiration = tokenExpiration,
                    message = "Logged in successfully"
                });

            }
            return Unauthorized("Invalid username or password");

        }
        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(string userName ,string roleName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null) return NotFound("This user does not exist in the system");

            if (roleName != "Admin" && roleName != "IT_Manager")
            {
                return BadRequest("in the system The submitted role is not supported");
            }
            var result = await userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new { message = $"Role ({roleName}) has been successfully assigned to user ({userName})!" });
            }
            return BadRequest(result.Errors);   
        }
    }
}
