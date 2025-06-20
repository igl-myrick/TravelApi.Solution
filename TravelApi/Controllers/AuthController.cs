using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
    public async Task<IActionResult> Register(Register registration)
    {
      ApplicationUser user = new ApplicationUser { UserName = registration.Username };
      IdentityResult result = await _userManager.CreateAsync(user, registration.Password);
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
    public async Task<IActionResult> Login(Login login)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, isPersistent: false, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        var accessToken = GenerateJSONWebToken();
        SetJWTCookie(accessToken);
        return Ok(accessToken);
      }
      else
      {
        return BadRequest("There is something wrong with your username or password.");
      }
    }

    private string GenerateJSONWebToken()
    {
      SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      JwtSecurityToken token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        expires: DateTime.Now.AddMinutes(10),
        signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private void SetJWTCookie(string token)
    {
      CookieOptions cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.Now.AddMinutes(10)
      };
      Response.Cookies.Append("jwtCookie", token, cookieOptions);
    }
  }
}