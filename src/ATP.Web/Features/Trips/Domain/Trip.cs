namespace ATP.Web.Features.Trips.Domain;

public class Trip
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Destination { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int NumberOfPeople { get; set; }
    public decimal Budget { get; set; }
    public string Objectives { get; set; } = string.Empty;
    public string AdditionalNotes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
