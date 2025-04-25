using System.ComponentModel.DataAnnotations;

namespace TravelApi.Models
{
  public class User
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$", ErrorMessage = "Your password must contain at least six characters, a capital letter, a lowercase letter, a number, and a special character.")]
    public string Password { get; set; }

    public User(string userName, string password)
    {
      UserName = userName;
      Password = password;
    }
  }
}