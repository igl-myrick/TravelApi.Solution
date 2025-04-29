using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TravelApi.Models
{
  public class ApplicationUser : IdentityUser
  {
    [Required]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$", ErrorMessage = "Your password must contain at least six characters, a capital letter, a lowercase letter, a number, and a special character.")]
    public string Password { get; set; }
  }
}