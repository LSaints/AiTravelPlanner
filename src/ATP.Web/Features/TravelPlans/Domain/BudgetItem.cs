namespace ATP.Web.Features.TravelPlans.Domain;

public class BudgetItem
{
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal EstimatedCost { get; set; }
}
