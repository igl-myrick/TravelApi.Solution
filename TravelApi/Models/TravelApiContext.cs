using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TravelApi.Models
{
  public class TravelApiContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Review> Reviews { get; set; }

    public TravelApiContext(DbContextOptions<TravelApiContext> options) : base(options) {}
  }
}