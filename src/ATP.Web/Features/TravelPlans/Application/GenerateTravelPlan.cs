using ATP.Web.Features.Trips.Application;
using ATP.Web.Features.TravelPlans.Domain;
using ATP.Web.Infrastructure.AI;
using ATP.Web.Shared.Abstractions;

namespace ATP.Web.Features.TravelPlans.Application;

public class GenerateTravelPlan
{
    private readonly ITripRepository _tripRepository;
    private readonly ITravelPlanRepository _planRepository;
    private readonly IAIProvider _aiProvider;

    public GenerateTravelPlan(
        ITripRepository tripRepository,
        ITravelPlanRepository planRepository,
        IAIProvider aiProvider)
    {
        _tripRepository = tripRepository;
        _planRepository = planRepository;
        _aiProvider = aiProvider;
    }

    public async Task<TravelPlan?> ExecuteAsync(Guid tripId)
    {
        var trip = await _tripRepository.GetByIdAsync(tripId);
        if (trip is null)
            return null;

        var plan = new TravelPlan
        {
            TripId = tripId,
            Prompt = PromptBuilder.Build(trip),
            Status = PlanStatus.Pending
        };

        await _planRepository.CreateAsync(plan);

        try
        {
            plan.Status = PlanStatus.Processing;

            var content = await _aiProvider.GenerateAsync(plan.Prompt, default);

            plan.GeneratedContent = content;
            plan.Status = PlanStatus.Completed;
        }
        catch
        {
            plan.Status = PlanStatus.Failed;
        }

        return plan;
    }
}
