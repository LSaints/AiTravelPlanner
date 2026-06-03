using Microsoft.Extensions.Logging;
using Moq;
using ATP.Web.Features.Trips.Application;
using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Tests.Features.Trips.Application;

public class DeleteTripTests
{
    [Fact]
    public async Task ShouldDeleteTripWhenExists()
    {
        var tripId = Guid.NewGuid();
        var existingTrip = new Trip
        {
            Id = tripId,
            Destination = "Paris",
            Country = "França"
        };

        var repo = new Mock<ITripRepository>();
        repo.Setup(r => r.GetByIdAsync(tripId)).ReturnsAsync(existingTrip);

        var logger = new Mock<ILogger<DeleteTrip>>();
        var useCase = new DeleteTrip(repo.Object, logger.Object);

        var result = await useCase.ExecuteAsync(tripId);

        Assert.True(result);
        repo.Verify(r => r.DeleteAsync(tripId), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnFalseWhenTripNotFound()
    {
        var repo = new Mock<ITripRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Trip?)null);

        var logger = new Mock<ILogger<DeleteTrip>>();
        var useCase = new DeleteTrip(repo.Object, logger.Object);

        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        Assert.False(result);
        repo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }
}
