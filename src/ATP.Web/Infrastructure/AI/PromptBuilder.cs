using System.Text;
using System.Text.Json;
using ATP.Web.Features.TravelPlans.Domain;
using ATP.Web.Features.Trips.Domain;

namespace ATP.Web.Infrastructure.AI;

public static class PromptBuilder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static string Build(Trip trip)
    {
        var sb = new StringBuilder();
        var totalDays = (trip.EndDate.DayNumber - trip.StartDate.DayNumber) + 1;

        sb.AppendLine("Aja como um Concierge de Viagens de luxo e Planejador de Itinerários Expert.");
        sb.AppendLine("Seu objetivo é criar um roteiro rico e extremamente prático.");
        sb.AppendLine();

        sb.AppendLine("# PARÂMETROS DA VIAGEM");
        sb.AppendLine($"- Destino: {trip.Destination}, {trip.Country}");
        sb.AppendLine($"- Duração: {totalDays} dias | Viajantes: {trip.NumberOfPeople}");
        sb.AppendLine($"- Orçamento Total: {trip.Budget:C} BRL | Perfil: {trip.Objectives}");
        sb.AppendLine();

        sb.AppendLine("### INSTRUÇÕES DE FORMATO (ESTRITAMENTE OBRIGATÓRIAS) ###");
        sb.AppendLine("Você DEVE retornar APENAS um JSON válido seguindo o schema abaixo. NÃO use formatação markdown, tabelas, blockquotes ou listas.");
        sb.AppendLine("O JSON deve conter obrigatoriamente as seguintes seções:");
        sb.AppendLine();
        sb.AppendLine("1. **StrategicSummary** — resumo executivo da logística e melhor forma de locomoção");
        sb.AppendLine("2. **Days** — lista de dias com atividades detalhadas (manhã, almoço, tarde, noite)");
        sb.AppendLine("3. **Budget** — lista com quebra de gastos por categoria");
        sb.AppendLine("4. **Checklist** — lista de itens essenciais e cuidados");
        sb.AppendLine("5. **Notes** (opcional) — notas personalizadas baseadas nas preferências do usuário");
        sb.AppendLine();
        sb.AppendLine("Schema JSON esperado:");
        sb.AppendLine("```");
        sb.AppendLine(GetSchemaExample(totalDays));
        sb.AppendLine("```");
        sb.AppendLine();

        sb.AppendLine("---");
        sb.AppendLine("## 🗺️ Planejamento Estratégico");
        sb.AppendLine("Forneça um resumo executivo da logística e a melhor forma de se locomover (apps de transporte, passe de metrô, etc).");
        sb.AppendLine();

        sb.AppendLine("## 🗓️ Itinerário Detalhado (Dia a Dia)");
        sb.AppendLine($"Crie um plano lógico para os {totalDays} dias, agrupando atrações por proximidade geográfica para otimizar o tempo.");
        sb.AppendLine();

        sb.AppendLine("## 💰 Distribuição Estimada do Orçamento");
        sb.AppendLine("Apresente a quebra de gastos baseada no orçamento fornecido.");
        sb.AppendLine();

        sb.AppendLine("## ⚠️ Checklist e Cuidados");
        sb.AppendLine("Inclua uma checklist de itens essenciais e cuidados de segurança/cultura local.");
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(trip.AdditionalNotes))
        {
            sb.AppendLine("## 📝 Notas Personalizadas");
            sb.AppendLine($"Considere estas preferências do usuário: {trip.AdditionalNotes}");
            sb.AppendLine();
        }

        sb.AppendLine("---");
        sb.AppendLine("**RESTRIÇÃO:** Não inclua introduções conversacionais. Retorne APENAS o JSON, sem markdown.");

        return sb.ToString();
    }

    private static string GetSchemaExample(int totalDays)
    {
        var example = new TravelPlanData
        {
            Summary = new StrategicSummary
            {
                Logistics = "Resumo da logística da viagem",
                Transportation = "Melhores opções de transporte",
                BestTimeToVisit = "Melhor época para visitar"
            },
            Days =
            [
                new DayItinerary
                {
                    Day = 1,
                    Title = "Chegada e Exploração",
                    Morning = "Atividade da manhã",
                    Lunch = "Sugestão de almoço",
                    Afternoon = "Atividade da tarde",
                    Evening = "Atividade da noite"
                }
            ],
            Budget =
            [
                new BudgetItem
                {
                    Category = "Hospedagem",
                    Description = "Descrição do gasto",
                    EstimatedCost = 1000m
                }
            ],
            Checklist =
            [
                new ChecklistItem
                {
                    Text = "Item da checklist",
                    Category = "Documentação",
                    IsEssential = true
                }
            ],
            Notes = null
        };

        return JsonSerializer.Serialize(example, JsonOptions);
    }
}
