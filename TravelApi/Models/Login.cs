using System.ComponentModel.DataAnnotations;

namespace TravelApi.Models
{
  public class Login
  {
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
  }
}