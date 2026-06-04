using ATP.Web.Features.TravelPlans.Domain;

namespace ATP.Web.Features.TravelPlans.Application;

public interface ITravelPlanRepository
{
    Task<TravelPlan?> GetByIdAsync(Guid id);
    Task<List<TravelPlan>> GetByTripAsync(Guid tripId);
    Task CreateAsync(TravelPlan plan);
    Task DeleteAsync(Guid id);
}
