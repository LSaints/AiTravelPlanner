using Microsoft.EntityFrameworkCore;
using ATP.Web.Features.Trips.Domain;
using ATP.Web.Features.Trips.Application;

namespace ATP.Web.Infrastructure.Persistence.Repositories;

public class TripRepository : ITripRepository
{
    private readonly ApplicationDbContext _context;

    public TripRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Trip?> GetByIdAsync(Guid id)
    {
        return await _context.Trips.FindAsync(id);
    }

    public async Task<List<Trip>> GetAllAsync()
    {
        return await _context.Trips.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task CreateAsync(Trip trip)
    {
        await _context.Trips.AddAsync(trip);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Trip trip)
    {
        _context.Trips.Update(trip);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var trip = await _context.Trips.FindAsync(id);
        if (trip is not null)
        {
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
        }
    }
}
