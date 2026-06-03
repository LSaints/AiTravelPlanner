using Microsoft.Extensions.Logging;
using Moq;
using ATP.Web.Features.Trips.Application;
using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Tests.Features.Trips.Application;

public class UpdateTripTests
{
    [Fact]
    public async Task ShouldUpdateTripWhenExists()
    {
        var tripId = Guid.NewGuid();
        var existingTrip = new Trip
        {
            Id = tripId,
            Destination = "Paris",
            Country = "França",
            NumberOfPeople = 1,
            Budget = 5000m
        };

        var repo = new Mock<ITripRepository>();
        repo.Setup(r => r.GetByIdAsync(tripId)).ReturnsAsync(existingTrip);

        var logger = new Mock<ILogger<UpdateTrip>>();
        var useCase = new UpdateTrip(repo.Object, logger.Object);

        var request = new UpdateTripRequest(
            tripId,
            "Londres", "Reino Unido",
            new DateOnly(2026, 8, 1), new DateOnly(2026, 8, 10),
            2, 8000m,
            "Turismo", "Hotel central");

        var result = await useCase.ExecuteAsync(request);

        Assert.NotNull(result);
        Assert.Equal("Londres", result.Destination);
        Assert.Equal("Reino Unido", result.Country);
        Assert.Equal(2, result.NumberOfPeople);
        Assert.Equal(8000m, result.Budget);

        repo.Verify(r => r.UpdateAsync(It.IsAny<Trip>()), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnNullWhenTripNotFound()
    {
        var repo = new Mock<ITripRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Trip?)null);

        var logger = new Mock<ILogger<UpdateTrip>>();
        var useCase = new UpdateTrip(repo.Object, logger.Object);

        var request = new UpdateTripRequest(
            Guid.NewGuid(),
            "Londres", "Reino Unido",
            new DateOnly(2026, 8, 1), new DateOnly(2026, 8, 10),
            2, 8000m,
            "Turismo", "");

        var result = await useCase.ExecuteAsync(request);

        Assert.Null(result);
        repo.Verify(r => r.UpdateAsync(It.IsAny<Trip>()), Times.Never);
    }
}
