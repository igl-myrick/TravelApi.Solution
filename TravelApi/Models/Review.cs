namespace TravelApi.Models
{
  public class Review
  {
    public int ReviewId { get; set; }
    public string Body { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string UserName { get; set; }
  }
}