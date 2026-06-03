namespace ATP.Web.Features.TravelPlans.Domain;

public class DayItinerary
{
    public int Day { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Morning { get; set; } = string.Empty;
    public string Lunch { get; set; } = string.Empty;
    public string Afternoon { get; set; } = string.Empty;
    public string Evening { get; set; } = string.Empty;
}
