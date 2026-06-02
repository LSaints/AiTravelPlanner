using Microsoft.EntityFrameworkCore;
using ATP.Web.Features.Trips.Domain;
using ATP.Web.Features.TravelPlans.Domain;

namespace ATP.Web.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<TravelPlan> TravelPlans => Set<TravelPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
