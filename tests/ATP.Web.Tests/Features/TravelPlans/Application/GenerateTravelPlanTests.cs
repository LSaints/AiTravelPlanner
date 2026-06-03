using Microsoft.Extensions.Logging;
using Moq;
using ATP.Web.Features.Trips.Application;
using ATP.Web.Features.Trips.Domain;
using ATP.Web.Features.TravelPlans.Application;
using ATP.Web.Features.TravelPlans.Domain;
using ATP.Web.Infrastructure.AI;
using ATP.Web.Shared.Abstractions;

namespace ATP.Web.Tests.Features.TravelPlans.Application;

public class GenerateTravelPlanTests
{
    [Fact]
    public async Task ShouldGeneratePlanSuccessfully()
    {
        var tripId = Guid.NewGuid();
        var trip = new Trip
        {
            Id = tripId,
            Destination = "Paris",
            Country = "França",
            StartDate = new DateOnly(2026, 7, 1),
            EndDate = new DateOnly(2026, 7, 10),
            NumberOfPeople = 2,
            Budget = 10000m,
            Objectives = "Turismo"
        };

        var tripRepo = new Mock<ITripRepository>();
        tripRepo.Setup(r => r.GetByIdAsync(tripId)).ReturnsAsync(trip);

        var planRepo = new Mock<ITravelPlanRepository>();
        TravelPlan? savedPlan = null;
        planRepo.Setup(r => r.CreateAsync(It.IsAny<TravelPlan>()))
            .Callback<TravelPlan>(p => savedPlan = p);

        var aiProvider = new Mock<IAIProvider>();
        aiProvider.Setup(p => p.GenerateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("# Plano de viagem para Paris");

        var logger = new Mock<ILogger<GenerateTravelPlan>>();
        var useCase = new GenerateTravelPlan(tripRepo.Object, planRepo.Object, aiProvider.Object, logger.Object);

        var result = await useCase.ExecuteAsync(tripId);

        Assert.NotNull(result);
        Assert.Equal("# Plano de viagem para Paris", result.GeneratedContent);
        Assert.Equal(PlanStatus.Completed, result.Status);

        planRepo.Verify(r => r.CreateAsync(It.IsAny<TravelPlan>()), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnNullWhenTripNotFound()
    {
        var tripRepo = new Mock<ITripRepository>();
        tripRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Trip?)null);

        var planRepo = new Mock<ITravelPlanRepository>();
        var aiProvider = new Mock<IAIProvider>();
        var logger = new Mock<ILogger<GenerateTravelPlan>>();
        var useCase = new GenerateTravelPlan(tripRepo.Object, planRepo.Object, aiProvider.Object, logger.Object);

        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        Assert.Null(result);
        planRepo.Verify(r => r.CreateAsync(It.IsAny<TravelPlan>()), Times.Never);
        aiProvider.Verify(p => p.GenerateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldSetFailedStatusWhenAiFails()
    {
        var tripId = Guid.NewGuid();
        var trip = new Trip
        {
            Id = tripId,
            Destination = "Londres",
            Country = "Reino Unido",
            StartDate = new DateOnly(2026, 6, 1),
            EndDate = new DateOnly(2026, 6, 5),
            NumberOfPeople = 1,
            Budget = 5000m,
            Objectives = "Lazer"
        };

        var tripRepo = new Mock<ITripRepository>();
        tripRepo.Setup(r => r.GetByIdAsync(tripId)).ReturnsAsync(trip);

        var planRepo = new Mock<ITravelPlanRepository>();

        var aiProvider = new Mock<IAIProvider>();
        aiProvider.Setup(p => p.GenerateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("API error"));

        var logger = new Mock<ILogger<GenerateTravelPlan>>();
        var useCase = new GenerateTravelPlan(tripRepo.Object, planRepo.Object, aiProvider.Object, logger.Object);

        var result = await useCase.ExecuteAsync(tripId);

        Assert.NotNull(result);
        Assert.Equal(PlanStatus.Failed, result.Status);
        Assert.Equal(string.Empty, result.GeneratedContent);
    }

    [Fact]
    public async Task ShouldCreatePlanWithPendingStatusInitially()
    {
        var tripId = Guid.NewGuid();
        var trip = new Trip
        {
            Id = tripId,
            Destination = "Roma",
            Country = "Itália",
            StartDate = new DateOnly(2026, 9, 1),
            EndDate = new DateOnly(2026, 9, 10),
            NumberOfPeople = 2,
            Budget = 8000m,
            Objectives = "Cultura"
        };

        var tripRepo = new Mock<ITripRepository>();
        tripRepo.Setup(r => r.GetByIdAsync(tripId)).ReturnsAsync(trip);

        PlanStatus? capturedStatus = null;
        var planRepo = new Mock<ITravelPlanRepository>();
        planRepo.Setup(r => r.CreateAsync(It.IsAny<TravelPlan>()))
            .Callback<TravelPlan>(p => capturedStatus = p.Status);

        var aiProvider = new Mock<IAIProvider>();
        aiProvider.Setup(p => p.GenerateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("Plano");

        var logger = new Mock<ILogger<GenerateTravelPlan>>();
        var useCase = new GenerateTravelPlan(tripRepo.Object, planRepo.Object, aiProvider.Object, logger.Object);

        await useCase.ExecuteAsync(tripId);

        Assert.Equal(PlanStatus.Pending, capturedStatus);
    }
}
