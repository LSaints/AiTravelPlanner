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
    private readonly ILogger<GenerateTravelPlan> _logger;

    public GenerateTravelPlan(
        ITripRepository tripRepository,
        ITravelPlanRepository planRepository,
        IAIProvider aiProvider,
        ILogger<GenerateTravelPlan> logger)
    {
        _tripRepository = tripRepository;
        _planRepository = planRepository;
        _aiProvider = aiProvider;
        _logger = logger;
    }

    public async Task<TravelPlan?> ExecuteAsync(Guid tripId)
    {
        var trip = await _tripRepository.GetByIdAsync(tripId);
        if (trip is null)
        {
            _logger.LogWarning("Tentativa de gerar plano para viagem inexistente. TripId: {TripId}", tripId);
            return null;
        }

        var plan = new TravelPlan
        {
            TripId = tripId,
            Prompt = PromptBuilder.Build(trip),
            Status = PlanStatus.Pending
        };

        await _planRepository.CreateAsync(plan);

        _logger.LogInformation(
            "Geração de plano iniciada. PlanId: {PlanId}, TripId: {TripId}, Destino: {Destination}",
            plan.Id, tripId, trip.Destination);

        try
        {
            plan.Status = PlanStatus.Processing;

            var content = await _aiProvider.GenerateAsync(plan.Prompt, default);

            plan.GeneratedContent = content;
            plan.Status = PlanStatus.Completed;

            _logger.LogInformation(
                "Plano gerado com sucesso. PlanId: {PlanId}, TripId: {TripId}",
                plan.Id, tripId);
        }
        catch (Exception ex)
        {
            plan.Status = PlanStatus.Failed;
            _logger.LogError(ex,
                "Falha na geração do plano. PlanId: {PlanId}, TripId: {TripId}",
                plan.Id, tripId);
        }

        return plan;
    }
}
