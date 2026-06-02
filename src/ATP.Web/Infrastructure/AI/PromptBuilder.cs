using System.Text;
using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Infrastructure.AI;

public static class PromptBuilder
{
    public static string Build(Trip trip)
    {
        var sb = new StringBuilder();

        var days = trip.EndDate.DayNumber - trip.StartDate.DayNumber;

        sb.AppendLine("Crie um plano de viagem completo.");
        sb.AppendLine();
        sb.AppendLine($"Destino: {trip.Destination}");
        sb.AppendLine($"País: {trip.Country}");
        sb.AppendLine($"Período: {days} dias");
        sb.AppendLine($"Pessoas: {trip.NumberOfPeople}");
        sb.AppendLine($"Orçamento: {trip.Budget:C}");
        sb.AppendLine();
        sb.AppendLine("Objetivo:");
        sb.AppendLine(trip.Objectives);

        if (!string.IsNullOrWhiteSpace(trip.AdditionalNotes))
        {
            sb.AppendLine();
            sb.AppendLine("Observações adicionais:");
            sb.AppendLine(trip.AdditionalNotes);
        }

        sb.AppendLine();
        sb.AppendLine("Retorne:");
        sb.AppendLine();
        sb.AppendLine("- Resumo");
        sb.AppendLine("- Custos estimados");
        sb.AppendLine("- Roteiro diário");
        sb.AppendLine("- Cuidados");
        sb.AppendLine("- Dicas locais");

        return sb.ToString();
    }
}
