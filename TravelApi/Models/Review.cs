using System.ComponentModel.DataAnnotations;

namespace TravelApi.Models
{
  public class Review
  {
    public int ReviewId { get; set; }
    [Required]
    public string Body { get; set; }
    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be a whole number between 1 and 5.")]
    public int Rating { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string UserName { get; set; }
  }
}