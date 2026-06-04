using Microsoft.Extensions.Logging;
using Moq;
using ATP.Web.Features.TravelPlans.Application;
using ATP.Web.Features.TravelPlans.Domain;

namespace ATP.Web.Tests.Features.TravelPlans.Application;

public class DeleteTravelPlanTests
{
    [Fact]
    public async Task ShouldDeleteTravelPlanWhenExists()
    {
        var planId = Guid.NewGuid();
        var existingPlan = new TravelPlan
        {
            Id = planId,
            TripId = Guid.NewGuid(),
            Status = PlanStatus.Completed
        };

        var repo = new Mock<ITravelPlanRepository>();
        repo.Setup(r => r.GetByIdAsync(planId)).ReturnsAsync(existingPlan);

        var logger = new Mock<ILogger<DeleteTravelPlan>>();
        var useCase = new DeleteTravelPlan(repo.Object, logger.Object);

        var result = await useCase.ExecuteAsync(planId);

        Assert.True(result);
        repo.Verify(r => r.DeleteAsync(planId), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnFalseWhenTravelPlanNotFound()
    {
        var repo = new Mock<ITravelPlanRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TravelPlan?)null);

        var logger = new Mock<ILogger<DeleteTravelPlan>>();
        var useCase = new DeleteTravelPlan(repo.Object, logger.Object);

        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        Assert.False(result);
        repo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task ShouldReturnFalseWhenTravelPlanIsProcessing()
    {
        var planId = Guid.NewGuid();
        var processingPlan = new TravelPlan
        {
            Id = planId,
            TripId = Guid.NewGuid(),
            Status = PlanStatus.Processing
        };

        var repo = new Mock<ITravelPlanRepository>();
        repo.Setup(r => r.GetByIdAsync(planId)).ReturnsAsync(processingPlan);

        var logger = new Mock<ILogger<DeleteTravelPlan>>();
        var useCase = new DeleteTravelPlan(repo.Object, logger.Object);

        var result = await useCase.ExecuteAsync(planId);

        Assert.False(result);
        repo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task ShouldDeleteTravelPlanWhenPending()
    {
        var planId = Guid.NewGuid();
        var pendingPlan = new TravelPlan
        {
            Id = planId,
            TripId = Guid.NewGuid(),
            Status = PlanStatus.Pending
        };

        var repo = new Mock<ITravelPlanRepository>();
        repo.Setup(r => r.GetByIdAsync(planId)).ReturnsAsync(pendingPlan);

        var logger = new Mock<ILogger<DeleteTravelPlan>>();
        var useCase = new DeleteTravelPlan(repo.Object, logger.Object);

        var result = await useCase.ExecuteAsync(planId);

        Assert.True(result);
        repo.Verify(r => r.DeleteAsync(planId), Times.Once);
    }

    [Fact]
    public async Task ShouldDeleteTravelPlanWhenFailed()
    {
        var planId = Guid.NewGuid();
        var failedPlan = new TravelPlan
        {
            Id = planId,
            TripId = Guid.NewGuid(),
            Status = PlanStatus.Failed
        };

        var repo = new Mock<ITravelPlanRepository>();
        repo.Setup(r => r.GetByIdAsync(planId)).ReturnsAsync(failedPlan);

        var logger = new Mock<ILogger<DeleteTravelPlan>>();
        var useCase = new DeleteTravelPlan(repo.Object, logger.Object);

        var result = await useCase.ExecuteAsync(planId);

        Assert.True(result);
        repo.Verify(r => r.DeleteAsync(planId), Times.Once);
    }
}
