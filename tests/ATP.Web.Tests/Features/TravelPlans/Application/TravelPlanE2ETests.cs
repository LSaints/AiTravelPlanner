using System.Text.Json;
using ATP.Web.Features.TravelPlans.Application;
using ATP.Web.Features.TravelPlans.Domain;
using ATP.Web.Features.Trips.Domain;
using ATP.Web.Infrastructure.AI;

namespace ATP.Web.Tests.Features.TravelPlans.Application;

public class TravelPlanE2ETests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    [Fact]
    public void PromptBuilder_GeneratesPromptWithJsonInstructions()
    {
        var trip = new Trip
        {
            Destination = "Tóquio",
            Country = "Japão",
            StartDate = new DateOnly(2026, 9, 1),
            EndDate = new DateOnly(2026, 9, 10),
            NumberOfPeople = 2,
            Budget = 15000m,
            Objectives = "Cultura e gastronomia"
        };

        var prompt = PromptBuilder.Build(trip);

        Assert.Contains("Tóquio", prompt);
        Assert.Contains("Japão", prompt);
        Assert.Contains("10 dias", prompt);
        Assert.Contains("JSON", prompt);
        Assert.Contains("StrategicSummary", prompt);
        Assert.Contains("Days", prompt);
        Assert.Contains("Budget", prompt);
        Assert.Contains("Checklist", prompt);
        Assert.Contains("Planejamento Estratégico", prompt);
        Assert.Contains("Itinerário Detalhado", prompt);
        Assert.Contains("Distribuição Estimada do Orçamento", prompt);
        Assert.Contains("Checklist e Cuidados", prompt);
        Assert.DoesNotContain("tabela Markdown", prompt);
        Assert.DoesNotContain("Blockquotes", prompt);
    }

    [Fact]
    public void FullPipeline_ValidJson_StoresAndParsesContent()
    {
        var sampleJson = """
        {
            "summary": {
                "logistics": "Voo direto GRU-NRT, 24h de viagem",
                "transportation": "JR Pass + Suica",
                "bestTimeToVisit": "Primavera"
            },
            "days": [
                {
                    "day": 1,
                    "title": "Chegada em Tóquio",
                    "morning": "Check-in no hotel em Shinjuku",
                    "lunch": "Ramen no Ichiran",
                    "afternoon": "Explorar Shibuya e Harajuku",
                    "evening": "Jantar no izakaya em Omoide Yokocho"
                }
            ],
            "budget": [
                {
                    "category": "Hospedagem",
                    "description": "Hotel 3 estrelas em Shinjuku",
                    "estimatedCost": 5000
                }
            ],
            "checklist": [
                {
                    "text": "Passaporte com validade mínima de 6 meses",
                    "category": "Documentação",
                    "isEssential": true
                }
            ],
            "notes": "Restrições alimentares: Preferência por comida sem glúten"
        }
        """;

        var plan = new TravelPlan
        {
            TripId = Guid.NewGuid(),
            GeneratedContent = sampleJson,
            Status = PlanStatus.Completed
        };

        Assert.Equal(PlanStatus.Completed, plan.Status);
        Assert.NotNull(plan.GeneratedContent);

        var parsed = TravelPlanParser.TryParse(plan.GeneratedContent, out var data);

        Assert.True(parsed);
        Assert.NotNull(data);
        Assert.Equal("Voo direto GRU-NRT, 24h de viagem", data.Summary.Logistics);
        Assert.Single(data.Days);
        Assert.Equal(1, data.Days[0].Day);
        Assert.Equal("Chegada em Tóquio", data.Days[0].Title);
        Assert.Single(data.Budget);
        Assert.Single(data.Checklist);
        Assert.NotNull(data.Notes);

        var plainText = TravelPlanParser.ToPlainText(data);

        Assert.Contains("RESUMO ESTRATÉGICO", plainText);
        Assert.Contains("ITINERÁRIO", plainText);
        Assert.Contains("ORÇAMENTO", plainText);
        Assert.Contains("CHECKLIST", plainText);
        Assert.Contains("NOTAS", plainText);
        Assert.Contains("[ESSENCIAL]", plainText);
    }

    [Fact]
    public void LegacyMarkdown_DetectedAsNonJson_FallbackPreserved()
    {
        var plan = new TravelPlan
        {
            GeneratedContent = """
            # Plano de Viagem para Paris

            ## Planejamento Estratégico
            Transporte público eficiente.

            ## Itinerário
            - Dia 1: Torre Eiffel
            """,
            Status = PlanStatus.Completed
        };

        var isJson = TravelPlanParser.TryParse(plan.GeneratedContent, out var data);

        Assert.False(isJson);
        Assert.Null(data);
    }

    [Fact]
    public void CorruptedContent_FallbackToMarkdown()
    {
        var corrupted = """
        {
            "summary": {
                "logistics": "Incompleto...
        """;

        var plan = new TravelPlan
        {
            GeneratedContent = corrupted,
            Status = PlanStatus.Completed
        };

        var isJson = TravelPlanParser.TryParse(plan.GeneratedContent, out var data);

        Assert.False(isJson);
        Assert.Null(data);
    }

    [Fact]
    public void CopiarPlano_CopiesPlainText_WhenStructured()
    {
        var data = new TravelPlanData
        {
            Summary = new StrategicSummary
            {
                Logistics = "Logística teste",
                Transportation = "Transporte teste",
                BestTimeToVisit = "Época teste"
            },
            Days =
            [
                new DayItinerary
                {
                    Day = 1,
                    Title = "Dia teste",
                    Morning = "Manhã teste",
                    Lunch = "Almoço teste",
                    Afternoon = "Tarde teste",
                    Evening = "Noite teste"
                }
            ],
            Budget =
            [
                new BudgetItem
                {
                    Category = "Categoria teste",
                    Description = "Descrição teste",
                    EstimatedCost = 100m
                }
            ],
            Checklist =
            [
                new ChecklistItem
                {
                    Text = "Item teste",
                    Category = "Categoria checklist",
                    IsEssential = true
                }
            ]
        };

        var copyText = TravelPlanParser.ToPlainText(data);

        Assert.Contains("Logística teste", copyText);
        Assert.Contains("Transporte teste", copyText);
        Assert.Contains("Manhã teste", copyText);
        Assert.Contains("Categoria teste", copyText);
        Assert.Contains("Item teste", copyText);
        Assert.DoesNotContain("{", copyText);
        Assert.DoesNotContain("summary", copyText);
    }

    [Fact]
    public void CopiarPlano_CopiesOriginalContent_WhenLegacyMarkdown()
    {
        var markdown = "# Plano de Viagem\n\n## Logística\nConteúdo original em markdown.";
        var plan = new TravelPlan
        {
            GeneratedContent = markdown,
            Status = PlanStatus.Completed
        };

        Assert.False(TravelPlanParser.TryParse(markdown, out _));

        var copyText = plan.GeneratedContent;

        Assert.Equal(markdown, copyText);
    }

    [Fact]
    public void ComponentSections_HaveAccessibleHeadings()
    {
        var data = new TravelPlanData
        {
            Summary = new StrategicSummary
            {
                Logistics = "X",
                Transportation = "Y",
                BestTimeToVisit = "Z"
            },
            Days =
            [
                new DayItinerary
                {
                    Day = 1,
                    Title = "Dia 1",
                    Morning = "M",
                    Lunch = "A",
                    Afternoon = "T",
                    Evening = "N"
                }
            ],
            Budget =
            [
                new BudgetItem
                {
                    Category = "C",
                    Description = "D",
                    EstimatedCost = 1m
                }
            ],
            Checklist =
            [
                new ChecklistItem
                {
                    Text = "I",
                    Category = "Cat",
                    IsEssential = true
                }
            ],
            Notes = "Nota: Conteúdo"
        };

        var text = TravelPlanParser.ToPlainText(data);

        Assert.Contains("RESUMO ESTRATÉGICO", text);
        Assert.Contains("ITINERÁRIO", text);
        Assert.Contains("Dia 1", text);
        Assert.Contains("ORÇAMENTO", text);
        Assert.Contains("CHECKLIST", text);
        Assert.Contains("NOTAS", text);
    }
}
