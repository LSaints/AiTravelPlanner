using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Features.Trips.Application;

public class GetTrip
{
    private readonly ITripRepository _repository;

    public GetTrip(ITripRepository repository)
    {
        _repository = repository;
    }

    public async Task<Trip?> ExecuteAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
}
