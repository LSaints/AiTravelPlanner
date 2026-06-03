namespace ATP.Web.Tests.Features.Trips.Domain;

public class TripTests
{
    [Fact]
    public void ShouldCreateTripWithDefaultValues()
    {
        var trip = new ATP.Web.Features.Trips.Domain.Trip();

        Assert.NotEqual(Guid.Empty, trip.Id);
        Assert.Equal(string.Empty, trip.Destination);
        Assert.Equal(string.Empty, trip.Country);
        Assert.Equal(0, trip.NumberOfPeople);
        Assert.Equal(0m, trip.Budget);
        Assert.Equal(string.Empty, trip.Objectives);
        Assert.Equal(string.Empty, trip.AdditionalNotes);
        Assert.NotEqual(default, trip.CreatedAt);
    }

    [Fact]
    public void ShouldAllowSettingProperties()
    {
        var trip = new ATP.Web.Features.Trips.Domain.Trip
        {
            Destination = "Paris",
            Country = "França",
            StartDate = new DateOnly(2026, 7, 10),
            EndDate = new DateOnly(2026, 7, 20),
            NumberOfPeople = 2,
            Budget = 10000m,
            Objectives = "Conhecer pontos turísticos",
            AdditionalNotes = "Preferência por hotéis centrais"
        };

        Assert.Equal("Paris", trip.Destination);
        Assert.Equal("França", trip.Country);
        Assert.Equal(new DateOnly(2026, 7, 10), trip.StartDate);
        Assert.Equal(new DateOnly(2026, 7, 20), trip.EndDate);
        Assert.Equal(2, trip.NumberOfPeople);
        Assert.Equal(10000m, trip.Budget);
        Assert.Equal("Conhecer pontos turísticos", trip.Objectives);
        Assert.Equal("Preferência por hotéis centrais", trip.AdditionalNotes);
    }

    [Fact]
    public void ShouldHaveImmutableIdAndCreatedAt()
    {
        var trip = new ATP.Web.Features.Trips.Domain.Trip();
        var id = trip.Id;
        var createdAt = trip.CreatedAt;

        Assert.Equal(id, trip.Id);
        Assert.Equal(createdAt, trip.CreatedAt);
    }
}
