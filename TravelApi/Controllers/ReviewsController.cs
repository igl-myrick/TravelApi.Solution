using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TravelApi.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TravelApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class ReviewsController : ControllerBase
  {
    private readonly TravelApiContext _db;

    public ReviewsController(TravelApiContext db)
    {
      _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> Get(string country, string city)
    {
      var jwt = Request.Cookies["jwtCookie"];

      IQueryable<Review> query = _db.Reviews.AsQueryable();

      if (country != null)
      {
        query = query.Where(entry => entry.Country == country);
      }

      if (city != null)
      {
        query = query.Where(entry => entry.City == city);
      }

      using (var httpClient = new HttpClient())
      {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        using (var response = await httpClient.GetAsync("http://localhost:5000/api/reviews/")) 
          {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
              string apiResponse = await response.Content.ReadAsStringAsync();
              query = (IQueryable<Review>)JsonConvert.DeserializeObject<List<Review>>(apiResponse);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
              return Unauthorized("Please Login again");
            }
          }
      }

      return await query.ToListAsync();
      }

    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetReview(int id)
    {
      Review review = await _db.Reviews.FindAsync(id);

      if (review == null)
      {
        return NotFound();
      }

      return review;
    }

    [HttpGet("random")]
    public async Task<ActionResult<Review>> GetRandom()
    {
      Random rand = new Random();
      int randomId = rand.Next(1, _db.Reviews.Count());
      Review reviewToDisplay = await _db.Reviews.FindAsync(randomId);
      return reviewToDisplay;
    }

    [HttpGet("popular")]
    public async Task<ActionResult<Dictionary<string, int>>> PopularResults()
    {
      List<Review> reviews = await _db.Reviews.ToListAsync();
      Dictionary<string, int> mostPopular = new Dictionary<string, int>{};
      foreach (Review review in reviews)
      {
        if (mostPopular.ContainsKey(review.City))
        {
          mostPopular[review.City]++;
        }
        else
        {
          mostPopular.Add(review.City, 1);
        }
      }
      Dictionary<string,int> orderedMostPopularCities = mostPopular.OrderByDescending(c => c.Value).ToDictionary(c => c.Key, c => c.Value);

      return orderedMostPopularCities;
    }

    [HttpGet("highest-rated")]
    public async Task<ActionResult<Dictionary<string,double>>> HighestRatedResults()
    {
      List<Review> reviews = await _db.Reviews.ToListAsync();
      List<string> cities = new List<string>{};
      foreach (Review review in reviews)
      {
        if (!cities.Contains(review.City))
        {
          cities.Add(review.City);
        }
      }

      Dictionary<string, double> highestRated = new Dictionary<string, double>{};
      foreach (string city in cities)
      {
        List<Review> cityReviews = _db.Reviews.Where(r => r.City == city).ToList();
        List<int> ratings = new List<int>{};
        foreach (Review review in cityReviews)
        {
          ratings.Add(review.Rating);
        }
        highestRated.Add(city, ratings.Average());
      }
      Dictionary<string,double> orderedHighestRated = highestRated.OrderByDescending(c => c.Value).ToDictionary(c => c.Key, c => c.Value);

      return orderedHighestRated;
    }

    [HttpPost]
    public async Task<ActionResult<Review>> Post(Review review)
    {
      _db.Reviews.Add(review);
      await _db.SaveChangesAsync();
      return CreatedAtAction(nameof(GetReview), new { id = review.ReviewId }, review);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, string userName, Review review)
    {
      if (id != review.ReviewId)
      {
        return BadRequest();
      }

      if (userName != review.UserName)
      {
        return BadRequest();
      }

      _db.Reviews.Update(review);

      try
      {
        await _db.SaveChangesAsync();
      }
      catch
      {
        if (!ReviewExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    private bool ReviewExists(int id)
    {
      return _db.Reviews.Any(e => e.ReviewId == id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id, string userName)
    {
      Review review = await _db.Reviews.FindAsync(id);
      if (review == null)
      {
        return NotFound();
      }

      if (userName != review.UserName)
      {
        return BadRequest();
      }

      _db.Reviews.Remove(review);
      await _db.SaveChangesAsync();

      return NoContent();
    }
  }
}