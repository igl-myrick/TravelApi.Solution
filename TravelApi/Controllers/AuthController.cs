using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelApi.Models;

namespace TravelApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly TravelApiContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TravelApiContext db, IConfiguration configuration)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
      _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
      ApplicationUser user = new ApplicationUser { UserName = model.Username };
      IdentityResult result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        return NoContent();
      }
      else
      {
        return BadRequest();
      }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        string issuer = _configuration["Jwt:Issuer"];
        string audience = _configuration["Jwt:Issuer"];
        byte[] key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        SigningCredentials signingCredentials = new SigningCredentials(
          new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha512Signature
        );

        ClaimsIdentity subject = new ClaimsIdentity(new[]
        {
          new Claim(JwtRegisteredClaimNames.Sub, model.Username),
          new Claim(JwtRegisteredClaimNames.Email, model.Username),
        });

        DateTime expires = DateTime.UtcNow.AddMinutes(10);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = subject,
          Expires = expires,
          Issuer = issuer,
          Audience = audience,
          SigningCredentials = signingCredentials
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return Ok(jwtToken);
      }
      else
      {
        return BadRequest("There is something wrong with your username or password.");
      }
    }
  }
}