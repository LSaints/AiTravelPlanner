using Microsoft.Extensions.Logging;
using Moq;
using ATP.Web.Features.Trips.Application;
using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Tests.Features.Trips.Application;

public class CreateTripTests
{
    [Fact]
    public async Task ShouldCreateTripAndCallRepository()
    {
        var repo = new Mock<ITripRepository>();
        var logger = new Mock<ILogger<CreateTrip>>();
        var useCase = new CreateTrip(repo.Object, logger.Object);

        var request = new CreateTripRequest(
            "Paris", "França",
            new DateOnly(2026, 7, 10), new DateOnly(2026, 7, 20),
            2, 10000m,
            "Turismo", "N/A");

        var result = await useCase.ExecuteAsync(request);

        Assert.NotNull(result);
        Assert.Equal("Paris", result.Destination);
        Assert.Equal("França", result.Country);

        repo.Verify(r => r.CreateAsync(It.IsAny<Trip>()), Times.Once);
    }

    [Fact]
    public async Task ShouldGenerateNewIdForEachTrip()
    {
        var repo = new Mock<ITripRepository>();
        var logger = new Mock<ILogger<CreateTrip>>();
        var useCase = new CreateTrip(repo.Object, logger.Object);

        var request1 = new CreateTripRequest(
            "Paris", "França",
            DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
            1, 1000m, "Lazer", "");

        var request2 = new CreateTripRequest(
            "Londres", "Reino Unido",
            DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
            1, 2000m, "Negócios", "");

        var trip1 = await useCase.ExecuteAsync(request1);
        var trip2 = await useCase.ExecuteAsync(request2);

        Assert.NotEqual(trip1.Id, trip2.Id);
    }
}
