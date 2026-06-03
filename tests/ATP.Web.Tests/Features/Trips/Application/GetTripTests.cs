using Moq;
using ATP.Web.Features.Trips.Application;
using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Tests.Features.Trips.Application;

public class GetTripTests
{
    [Fact]
    public async Task ShouldReturnTripWhenFound()
    {
        var tripId = Guid.NewGuid();
        var expectedTrip = new Trip
        {
            Id = tripId,
            Destination = "Paris",
            Country = "França"
        };

        var repo = new Mock<ITripRepository>();
        repo.Setup(r => r.GetByIdAsync(tripId)).ReturnsAsync(expectedTrip);

        var useCase = new GetTrip(repo.Object);

        var result = await useCase.ExecuteAsync(tripId);

        Assert.NotNull(result);
        Assert.Equal(tripId, result.Id);
        Assert.Equal("Paris", result.Destination);
    }

    [Fact]
    public async Task ShouldReturnNullWhenNotFound()
    {
        var repo = new Mock<ITripRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Trip?)null);

        var useCase = new GetTrip(repo.Object);

        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        Assert.Null(result);
    }
}
