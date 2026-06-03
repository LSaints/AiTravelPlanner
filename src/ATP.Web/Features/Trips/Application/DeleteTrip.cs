using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Features.Trips.Application;

public class DeleteTrip
{
    private readonly ITripRepository _repository;
    private readonly ILogger<DeleteTrip> _logger;

    public DeleteTrip(ITripRepository repository, ILogger<DeleteTrip> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> ExecuteAsync(Guid id)
    {
        var trip = await _repository.GetByIdAsync(id);
        if (trip is null)
        {
            _logger.LogWarning("Tentativa de excluir viagem inexistente. Id: {TripId}", id);
            return false;
        }

        await _repository.DeleteAsync(id);

        _logger.LogInformation(
            "Viagem excluída. Id: {TripId}, Destino: {Destination}, País: {Country}",
            trip.Id, trip.Destination, trip.Country);

        return true;
    }
}
