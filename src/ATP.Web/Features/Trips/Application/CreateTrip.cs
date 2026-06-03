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
    private readonly ILogger<CreateTrip> _logger;

    public CreateTrip(ITripRepository repository, ILogger<CreateTrip> logger)
    {
        _repository = repository;
        _logger = logger;
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

        _logger.LogInformation(
            "Viagem criada. Id: {TripId}, Destino: {Destination}, País: {Country}, Período: {StartDate} a {EndDate}, Pessoas: {People}, Orçamento: {Budget}",
            trip.Id, trip.Destination, trip.Country, trip.StartDate, trip.EndDate, trip.NumberOfPeople, trip.Budget);

        return trip;
    }
}
