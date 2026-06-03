namespace ATP.Web.Features.TravelPlans.Domain;

public class ChecklistItem
{
    public string Text { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsEssential { get; set; }
}
