using ATP.Web.Features.TravelPlans.Application;
using ATP.Web.Features.TravelPlans.Domain;

namespace ATP.Web.Tests.Features.TravelPlans.Application;

public class TravelPlanParserTests
{
    [Fact]
    public void TryParse_ValidJson_ReturnsTrueAndPopulatesData()
    {
        var json = """
        {
            "summary": {
                "logistics": "Voo direto GRU-LIS",
                "transportation": "Metro e Uber",
                "bestTimeToVisit": "Primavera"
            },
            "days": [
                {
                    "day": 1,
                    "title": "Chegada",
                    "morning": "Check-in no hotel",
                    "lunch": "Pastel de nata",
                    "afternoon": "Passeio pela Baixa",
                    "evening": "Jantar no Time Out Market"
                }
            ],
            "budget": [
                {
                    "category": "Hospedagem",
                    "description": "Hotel 4 estrelas",
                    "estimatedCost": 3000
                }
            ],
            "checklist": [
                {
                    "text": "Passaporte",
                    "category": "Documentação",
                    "isEssential": true
                }
            ],
            "notes": "Preferências: Evitar restaurantes turísticos"
        }
        """;

        var result = TravelPlanParser.TryParse(json, out var data);

        Assert.True(result);
        Assert.NotNull(data);
        Assert.Equal("Voo direto GRU-LIS", data.Summary.Logistics);
        Assert.Equal("Metro e Uber", data.Summary.Transportation);
        Assert.Equal("Primavera", data.Summary.BestTimeToVisit);
        Assert.Single(data.Days);
        Assert.Equal(1, data.Days[0].Day);
        Assert.Equal("Chegada", data.Days[0].Title);
        Assert.Equal("Check-in no hotel", data.Days[0].Morning);
        Assert.Equal("Pastel de nata", data.Days[0].Lunch);
        Assert.Equal("Passeio pela Baixa", data.Days[0].Afternoon);
        Assert.Equal("Jantar no Time Out Market", data.Days[0].Evening);
        Assert.Single(data.Budget);
        Assert.Equal("Hospedagem", data.Budget[0].Category);
        Assert.Equal("Hotel 4 estrelas", data.Budget[0].Description);
        Assert.Equal(3000, data.Budget[0].EstimatedCost);
        Assert.Single(data.Checklist);
        Assert.Equal("Passaporte", data.Checklist[0].Text);
        Assert.Equal("Documentação", data.Checklist[0].Category);
        Assert.True(data.Checklist[0].IsEssential);
        Assert.NotNull(data.Notes);
        Assert.Equal("Preferências: Evitar restaurantes turísticos", data.Notes);
    }

    [Fact]
    public void TryParse_ValidJsonWithMissingFields_PopulatesDefaults()
    {
        var json = """
        {
            "summary": {
                "logistics": "Logística básica",
                "transportation": "",
                "bestTimeToVisit": null
            },
            "days": [],
            "budget": [],
            "checklist": []
        }
        """;

        var result = TravelPlanParser.TryParse(json, out var data);

        Assert.True(result);
        Assert.NotNull(data);
        Assert.Equal("Logística básica", data.Summary.Logistics);
        Assert.Equal("", data.Summary.Transportation);
        Assert.Null(data.Summary.BestTimeToVisit);
        Assert.Empty(data.Days);
        Assert.Empty(data.Budget);
        Assert.Empty(data.Checklist);
        Assert.Null(data.Notes);
    }

    [Fact]
    public void TryParse_EmptyString_ReturnsFalse()
    {
        var result = TravelPlanParser.TryParse("", out var data);

        Assert.False(result);
        Assert.Null(data);
    }

    [Fact]
    public void TryParse_WhitespaceString_ReturnsFalse()
    {
        var result = TravelPlanParser.TryParse("   ", out var data);

        Assert.False(result);
        Assert.Null(data);
    }

    [Fact]
    public void TryParse_Markdown_ReturnsFalse()
    {
        var markdown = """
        # Plano de Viagem
        ## Logística
        Voo direto
        """;

        var result = TravelPlanParser.TryParse(markdown, out var data);

        Assert.False(result);
        Assert.Null(data);
    }

    [Fact]
    public void TryParse_InvalidJson_ReturnsFalse()
    {
        var json = "{ summary: }";

        var result = TravelPlanParser.TryParse(json, out var data);

        Assert.False(result);
        Assert.Null(data);
    }

    [Fact]
    public void TryParse_MarkdownStartingWithBrace_ReturnsFalse()
    {
        var markdown = "{Este é um texto que começa com chave} mas não é JSON";

        var result = TravelPlanParser.TryParse(markdown, out var data);

        Assert.False(result);
        Assert.Null(data);
    }

    [Fact]
    public void TryParse_JsonInCodeBlock_ExtractsAndParses()
    {
        var content = """
        Aqui está o plano solicitado:

        ```json
        {
            "summary": {
                "logistics": "Voo direto GRU-LIS",
                "transportation": "Metro e Uber",
                "bestTimeToVisit": "Primavera"
            },
            "days": [
                {
                    "day": 1,
                    "title": "Chegada",
                    "morning": "Check-in",
                    "lunch": "Pastel de nata",
                    "afternoon": "Passeio",
                    "evening": "Jantar"
                }
            ],
            "budget": [
                {
                    "category": "Hospedagem",
                    "description": "Hotel",
                    "estimatedCost": 2000
                }
            ],
            "checklist": [
                {
                    "text": "Passaporte",
                    "category": "Docs",
                    "isEssential": true
                }
            ]
        }
        ```

        Espero que aproveite!
        """;

        var result = TravelPlanParser.TryParse(content, out var data);

        Assert.True(result);
        Assert.NotNull(data);
        Assert.Equal("Voo direto GRU-LIS", data.Summary.Logistics);
        Assert.Single(data.Days);
        Assert.Single(data.Budget);
        Assert.Single(data.Checklist);
    }

    [Fact]
    public void TryParse_JsonInCodeBlockWithoutLanguage_ExtractsAndParses()
    {
        var content = """
        ```
        {"summary":{"logistics":"Teste","transportation":"Onibus","bestTimeToVisit":"Verao"},"days":[{"day":1,"title":"Dia 1","morning":"M","lunch":"A","afternoon":"T","evening":"N"}],"budget":[],"checklist":[]}
        ```
        """;

        var result = TravelPlanParser.TryParse(content, out var data);

        Assert.True(result);
        Assert.NotNull(data);
        Assert.Equal("Teste", data.Summary.Logistics);
    }

    [Fact]
    public void ToPlainText_IncludesAllSections()
    {
        var data = new TravelPlanData
        {
            Summary = new StrategicSummary
            {
                Logistics = "Voo direto",
                Transportation = "Metro",
                BestTimeToVisit = "Verão"
            },
            Days =
            [
                new DayItinerary
                {
                    Day = 1,
                    Title = "Exploração",
                    Morning = "Museu",
                    Lunch = "Restaurante local",
                    Afternoon = "Parque",
                    Evening = "Jantar"
                }
            ],
            Budget =
            [
                new BudgetItem
                {
                    Category = "Alimentação",
                    Description = "Refeições",
                    EstimatedCost = 500m
                }
            ],
            Checklist =
            [
                new ChecklistItem
                {
                    Text = "Protetor solar",
                    Category = "Saúde",
                    IsEssential = true
                },
                new ChecklistItem
                {
                    Text = "Livro",
                    Category = "Lazer",
                    IsEssential = false
                }
            ],
            Notes = "Dica local: Experimentar a culinária de rua"
        };

        var text = TravelPlanParser.ToPlainText(data);

        Assert.Contains("RESUMO ESTRATÉGICO", text);
        Assert.Contains("Voo direto", text);
        Assert.Contains("Metro", text);
        Assert.Contains("Verão", text);
        Assert.Contains("ITINERÁRIO", text);
        Assert.Contains("Dia 1", text);
        Assert.Contains("Exploração", text);
        Assert.Contains("Museu", text);
        Assert.Contains("Restaurante local", text);
        Assert.Contains("Parque", text);
        Assert.Contains("Jantar", text);
        Assert.Contains("ORÇAMENTO", text);
        Assert.Contains("Alimentação", text);
        Assert.Contains("Refeições", text);
        Assert.Contains("CHECKLIST", text);
        Assert.Contains("Protetor solar", text);
        Assert.Contains("[ESSENCIAL]", text);
        Assert.Contains("Livro", text);
        Assert.DoesNotContain("[ESSENCIAL]", text.Split("Livro").Last());
        Assert.Contains("NOTAS", text);
        Assert.Contains("Dica local: Experimentar a culinária de rua", text);
    }

    [Fact]
    public void ToPlainText_WithoutNotes_OmitsNotesSection()
    {
        var data = new TravelPlanData
        {
            Summary = new StrategicSummary
            {
                Logistics = "Ônibus",
                Transportation = "Trem",
                BestTimeToVisit = "Inverno"
            },
            Days = [],
            Budget = [],
            Checklist = []
        };

        var text = TravelPlanParser.ToPlainText(data);

        Assert.Contains("RESUMO ESTRATÉGICO", text);
        Assert.DoesNotContain("NOTAS", text);
    }
}
