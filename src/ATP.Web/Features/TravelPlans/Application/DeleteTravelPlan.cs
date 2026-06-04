using ATP.Web.Features.TravelPlans.Domain;

namespace ATP.Web.Features.TravelPlans.Application;

public class DeleteTravelPlan
{
    private readonly ITravelPlanRepository _repository;
    private readonly ILogger<DeleteTravelPlan> _logger;

    public DeleteTravelPlan(ITravelPlanRepository repository, ILogger<DeleteTravelPlan> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> ExecuteAsync(Guid id)
    {
        var plan = await _repository.GetByIdAsync(id);
        if (plan is null)
        {
            _logger.LogWarning("Tentativa de excluir plano de viagem inexistente. Id: {PlanId}", id);
            return false;
        }

        if (plan.Status == PlanStatus.Processing)
        {
            _logger.LogWarning(
                "Tentativa de excluir plano de viagem em processamento. Id: {PlanId}", id);
            return false;
        }

        await _repository.DeleteAsync(id);

        _logger.LogInformation(
            "Plano de viagem excluído. Id: {PlanId}, TripId: {TripId}",
            plan.Id, plan.TripId);

        return true;
    }
}
