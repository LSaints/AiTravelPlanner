using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Features.Trips.Application;

public record CreateTripRequest(
    string Destination,
    string Country,
    DateOnly StartDate,
    DateOnly EndDate,
    int NumberOfPeople,
    decimal Budget,
    string Objectives,
    string AdditionalNotes
);

public class CreateTrip
{
    private readonly ITripRepository _repository;

    public CreateTrip(ITripRepository repository)
    {
        _repository = repository;
    }

    public async Task<Trip> ExecuteAsync(CreateTripRequest request)
    {
        var trip = new Trip
        {
            Destination = request.Destination,
            Country = request.Country,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            NumberOfPeople = request.NumberOfPeople,
            Budget = request.Budget,
            Objectives = request.Objectives,
            AdditionalNotes = request.AdditionalNotes
        };

        await _repository.CreateAsync(trip);
        return trip;
    }
}
