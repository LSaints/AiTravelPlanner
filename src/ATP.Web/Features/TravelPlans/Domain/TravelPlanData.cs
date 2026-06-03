namespace ATP.Web.Features.TravelPlans.Domain;

public class TravelPlanData
{
    public StrategicSummary Summary { get; set; } = new();
    public List<DayItinerary> Days { get; set; } = [];
    public List<BudgetItem> Budget { get; set; } = [];
    public List<ChecklistItem> Checklist { get; set; } = [];
    public string? Notes { get; set; }
}
