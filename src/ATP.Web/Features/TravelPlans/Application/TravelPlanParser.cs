using System.Text.Json;
using ATP.Web.Features.TravelPlans.Domain;

namespace ATP.Web.Features.TravelPlans.Application;

public static class TravelPlanParser
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static bool TryParse(string content, out TravelPlanData? data)
    {
        data = null;

        if (string.IsNullOrWhiteSpace(content))
            return false;

        var trimmed = content.Trim();

        if (trimmed.StartsWith('{') == false)
        {
            trimmed = ExtractJsonFromCodeBlock(trimmed);
            if (trimmed is null)
                return false;
        }

        try
        {
            data = JsonSerializer.Deserialize<TravelPlanData>(trimmed, JsonOptions);
            return data is not null;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static string? ExtractJsonFromCodeBlock(string content)
    {
        var startMarkers = new[] { "```json", "```" };

        foreach (var marker in startMarkers)
        {
            var startIdx = content.IndexOf(marker, StringComparison.Ordinal);
            if (startIdx < 0) continue;

            var jsonStart = startIdx + marker.Length;
            var endIdx = content.IndexOf("```", jsonStart, StringComparison.Ordinal);
            if (endIdx < 0) continue;

            var extracted = content[jsonStart..endIdx].Trim();
            if (extracted.StartsWith('{'))
                return extracted;
        }

        return null;
    }

    public static string ToPlainText(TravelPlanData data)
    {
        var lines = new List<string>
        {
            "=== RESUMO ESTRATÉGICO ===",
            $"Logística: {data.Summary.Logistics}",
            $"Transporte: {data.Summary.Transportation}",
            $"Melhor época: {data.Summary.BestTimeToVisit}",
            "",
            "=== ITINERÁRIO ==="
        };

        foreach (var day in data.Days)
        {
            lines.Add($"\nDia {day.Day} — {day.Title}");
            lines.Add($"  Manhã: {day.Morning}");
            lines.Add($"  Almoço: {day.Lunch}");
            lines.Add($"  Tarde: {day.Afternoon}");
            lines.Add($"  Noite: {day.Evening}");
        }

        lines.Add("\n=== ORÇAMENTO ===");
        foreach (var item in data.Budget)
        {
            lines.Add($"{item.Category} | {item.Description} | {item.EstimatedCost:C}");
        }

        lines.Add("\n=== CHECKLIST ===");
        foreach (var item in data.Checklist)
        {
            var essential = item.IsEssential ? " [ESSENCIAL]" : "";
            lines.Add($"- {item.Text} ({item.Category}){essential}");
        }

        if (!string.IsNullOrWhiteSpace(data.Notes))
        {
            lines.Add("\n=== NOTAS ===");
            lines.Add($"  {data.Notes}");
        }

        return string.Join('\n', lines);
    }
}
