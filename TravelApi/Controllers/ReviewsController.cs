using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelApi.Models;

namespace TravelApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
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
      IQueryable<Review> query = _db.Reviews.AsQueryable();

      if (country != null)
      {
        query = query.Where(entry => entry.Country == country);
      }

      if (city != null)
      {
        query = query.Where(entry => entry.City == city);
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
    public async Task<ActionResult<Dictionary<string,double>>> GetPopular()
    {
      List<Review> reviews = _db.Reviews.ToList();
      List<string> cities = new List<string>{};
      if (reviews != null)
      {
        foreach (Review review in reviews)
        {
          cities.Add(review.City);
        }
      }
      Dictionary<string, int> mostPopular = cities.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

      Dictionary<string, double> highestRated = new Dictionary<string, double>{};
      foreach (string key in mostPopular.Keys)
      {
        List<Review> cityReviews = _db.Reviews.Where(r => r.City == key).ToList();
        List<int> ratings = new List<int>{};
        foreach (Review review in cityReviews)
        {
          ratings.Add(review.Rating);
        }
        highestRated.Add(key, ratings.Average());
      }

      return highestRated;
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