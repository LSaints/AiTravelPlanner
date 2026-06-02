using Microsoft.EntityFrameworkCore;
using ATP.Web.Features.TravelPlans.Domain;
using ATP.Web.Features.TravelPlans.Application;

namespace ATP.Web.Infrastructure.Persistence.Repositories;

public class TravelPlanRepository : ITravelPlanRepository
{
    private readonly ApplicationDbContext _context;

    public TravelPlanRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TravelPlan?> GetByIdAsync(Guid id)
    {
        return await _context.TravelPlans.FindAsync(id);
    }

    public async Task<List<TravelPlan>> GetByTripAsync(Guid tripId)
    {
        return await _context.TravelPlans
            .Where(tp => tp.TripId == tripId)
            .OrderByDescending(tp => tp.GeneratedAt)
            .ToListAsync();
    }

    public async Task CreateAsync(TravelPlan plan)
    {
        await _context.TravelPlans.AddAsync(plan);
        await _context.SaveChangesAsync();
    }
}
