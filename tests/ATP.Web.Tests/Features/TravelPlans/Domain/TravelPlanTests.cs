using ATP.Web.Features.TravelPlans.Domain;

namespace ATP.Web.Tests.Features.TravelPlans.Domain;

public class TravelPlanTests
{
    [Fact]
    public void ShouldCreateTravelPlanWithDefaultValues()
    {
        var plan = new TravelPlan();

        Assert.NotEqual(Guid.Empty, plan.Id);
        Assert.Equal(Guid.Empty, plan.TripId);
        Assert.Equal(string.Empty, plan.Prompt);
        Assert.Equal(string.Empty, plan.GeneratedContent);
        Assert.Equal(string.Empty, plan.PromptVersion);
        Assert.Equal(string.Empty, plan.AiProvider);
        Assert.Equal(string.Empty, plan.Model);
        Assert.Null(plan.TokensUsed);
        Assert.Null(plan.EstimatedCost);
        Assert.Equal(PlanStatus.Pending, plan.Status);
        Assert.NotEqual(default, plan.GeneratedAt);
    }

    [Fact]
    public void ShouldAllowSettingAllProperties()
    {
        var tripId = Guid.NewGuid();
        var plan = new TravelPlan
        {
            TripId = tripId,
            Prompt = "Crie um plano...",
            GeneratedContent = "# Plano completo",
            PromptVersion = "v1",
            AiProvider = "OpenAI",
            Model = "gpt-4o-mini",
            TokensUsed = 500,
            EstimatedCost = 0.01m,
            Status = PlanStatus.Completed
        };

        Assert.Equal(tripId, plan.TripId);
        Assert.Equal("Crie um plano...", plan.Prompt);
        Assert.Equal("# Plano completo", plan.GeneratedContent);
        Assert.Equal("v1", plan.PromptVersion);
        Assert.Equal("OpenAI", plan.AiProvider);
        Assert.Equal("gpt-4o-mini", plan.Model);
        Assert.Equal(500, plan.TokensUsed);
        Assert.Equal(0.01m, plan.EstimatedCost);
        Assert.Equal(PlanStatus.Completed, plan.Status);
    }

    [Fact]
    public void ShouldDefaultStatusToPending()
    {
        var plan = new TravelPlan();
        Assert.Equal(PlanStatus.Pending, plan.Status);
    }
}
