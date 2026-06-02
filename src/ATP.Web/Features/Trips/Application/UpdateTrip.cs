using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Features.Trips.Application;

public record UpdateTripRequest(
    Guid Id,
    string Destination,
    string Country,
    DateOnly StartDate,
    DateOnly EndDate,
    int NumberOfPeople,
    decimal Budget,
    string Objectives,
    string AdditionalNotes
);

public class UpdateTrip
{
    private readonly ITripRepository _repository;

    public UpdateTrip(ITripRepository repository)
    {
        _repository = repository;
    }

    public async Task<Trip?> ExecuteAsync(UpdateTripRequest request)
    {
        var trip = await _repository.GetByIdAsync(request.Id);
        if (trip is null)
            return null;

        trip.Destination = request.Destination;
        trip.Country = request.Country;
        trip.StartDate = request.StartDate;
        trip.EndDate = request.EndDate;
        trip.NumberOfPeople = request.NumberOfPeople;
        trip.Budget = request.Budget;
        trip.Objectives = request.Objectives;
        trip.AdditionalNotes = request.AdditionalNotes;

        await _repository.UpdateAsync(trip);
        return trip;
    }
}
