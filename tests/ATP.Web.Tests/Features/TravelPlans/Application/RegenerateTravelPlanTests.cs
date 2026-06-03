using Microsoft.Extensions.Logging;
using Moq;
using ATP.Web.Features.Trips.Application;
using ATP.Web.Features.Trips.Domain;
using ATP.Web.Features.TravelPlans.Application;
using ATP.Web.Features.TravelPlans.Domain;
using ATP.Web.Shared.Abstractions;

namespace ATP.Web.Tests.Features.TravelPlans.Application;

public class RegenerateTravelPlanTests
{
    [Fact]
    public async Task ShouldCreateNewPlanAndNotOverwritePrevious()
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

        var createdPlans = new List<TravelPlan>();
        var planRepo = new Mock<ITravelPlanRepository>();
        planRepo.Setup(r => r.CreateAsync(It.IsAny<TravelPlan>()))
            .Callback<TravelPlan>(p => createdPlans.Add(p));

        var aiProvider = new Mock<IAIProvider>();
        aiProvider.Setup(p => p.GenerateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("# Plano gerado");

        var geminiMetadata = new Mock<IGeminiMetadata>();
        geminiMetadata.Setup(m => m.ProviderName).Returns("Gemini");
        geminiMetadata.Setup(m => m.ModelName).Returns("gemini-2.0-flash");

        var logger = new Mock<ILogger<RegenerateTravelPlan>>();
        var useCase = new RegenerateTravelPlan(tripRepo.Object, planRepo.Object, aiProvider.Object, geminiMetadata.Object, logger.Object);

        // Generate first plan
        var plan1 = await useCase.ExecuteAsync(tripId);
        // Regenerate
        var plan2 = await useCase.ExecuteAsync(tripId);

        Assert.NotNull(plan1);
        Assert.NotNull(plan2);
        Assert.NotEqual(plan1!.Id, plan2!.Id);
        Assert.Equal(2, createdPlans.Count);
    }

    [Fact]
    public async Task ShouldReturnNullWhenTripNotFound()
    {
        var tripRepo = new Mock<ITripRepository>();
        tripRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Trip?)null);

        var planRepo = new Mock<ITravelPlanRepository>();
        var aiProvider = new Mock<IAIProvider>();
        var geminiMetadata = new Mock<IGeminiMetadata>();
        var logger = new Mock<ILogger<RegenerateTravelPlan>>();
        var useCase = new RegenerateTravelPlan(tripRepo.Object, planRepo.Object, aiProvider.Object, geminiMetadata.Object, logger.Object);

        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task ShouldSetFailedStatusWhenAiFails()
    {
        var tripId = Guid.NewGuid();
        var trip = new Trip
        {
            Id = tripId,
            Destination = "Berlim",
            Country = "Alemanha",
            StartDate = new DateOnly(2026, 8, 1),
            EndDate = new DateOnly(2026, 8, 5),
            NumberOfPeople = 1,
            Budget = 3000m,
            Objectives = "Negócios"
        };

        var tripRepo = new Mock<ITripRepository>();
        tripRepo.Setup(r => r.GetByIdAsync(tripId)).ReturnsAsync(trip);

        var planRepo = new Mock<ITravelPlanRepository>();

        var aiProvider = new Mock<IAIProvider>();
        aiProvider.Setup(p => p.GenerateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Timeout"));

        var geminiMetadata = new Mock<IGeminiMetadata>();
        geminiMetadata.Setup(m => m.ProviderName).Returns("Gemini");
        geminiMetadata.Setup(m => m.ModelName).Returns("gemini-2.0-flash");

        var logger = new Mock<ILogger<RegenerateTravelPlan>>();
        var useCase = new RegenerateTravelPlan(tripRepo.Object, planRepo.Object, aiProvider.Object, geminiMetadata.Object, logger.Object);

        var result = await useCase.ExecuteAsync(tripId);

        Assert.NotNull(result);
        Assert.Equal(PlanStatus.Failed, result.Status);
    }
}
