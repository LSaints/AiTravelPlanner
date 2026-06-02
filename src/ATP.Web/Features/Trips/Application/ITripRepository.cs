using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Features.Trips.Application;

public interface ITripRepository
{
    Task<Trip?> GetByIdAsync(Guid id);
    Task<List<Trip>> GetAllAsync();
    Task CreateAsync(Trip trip);
    Task UpdateAsync(Trip trip);
    Task DeleteAsync(Guid id);
}
