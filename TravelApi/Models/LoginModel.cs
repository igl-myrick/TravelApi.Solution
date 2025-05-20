using System.ComponentModel.DataAnnotations;

namespace TravelApi.Models
{
  public class LoginModel
  {
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
  }
}