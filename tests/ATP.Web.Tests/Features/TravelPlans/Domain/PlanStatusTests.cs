namespace ATP.Web.Tests.Features.TravelPlans.Domain;

public class PlanStatusTests
{
    [Fact]
    public void ShouldHaveExpectedValues()
    {
        Assert.Equal(0, (int)ATP.Web.Features.TravelPlans.Domain.PlanStatus.Pending);
        Assert.Equal(1, (int)ATP.Web.Features.TravelPlans.Domain.PlanStatus.Processing);
        Assert.Equal(2, (int)ATP.Web.Features.TravelPlans.Domain.PlanStatus.Completed);
        Assert.Equal(3, (int)ATP.Web.Features.TravelPlans.Domain.PlanStatus.Failed);
    }
}
