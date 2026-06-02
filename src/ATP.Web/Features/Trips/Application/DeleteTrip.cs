using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Features.Trips.Application;

public class DeleteTrip
{
    private readonly ITripRepository _repository;

    public DeleteTrip(ITripRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(Guid id)
    {
        var trip = await _repository.GetByIdAsync(id);
        if (trip is null)
            return false;

        await _repository.DeleteAsync(id);
        return true;
    }
}
