namespace ATP.Web.Features.TravelPlans.Domain;

public class TravelPlan
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid TripId { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string GeneratedContent { get; set; } = string.Empty;
    public string PromptVersion { get; set; } = string.Empty;
    public string AiProvider { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int? TokensUsed { get; set; }
    public decimal? EstimatedCost { get; set; }
    public PlanStatus Status { get; set; }
    public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
}
