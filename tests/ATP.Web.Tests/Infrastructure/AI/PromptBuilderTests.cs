using System.Globalization;
using ATP.Web.Features.Trips.Domain;
using ATP.Web.Infrastructure.AI;

namespace ATP.Web.Tests.Infrastructure.AI;

public class PromptBuilderTests
{
    [Fact]
    public void ShouldIncludeDestinationAndCountry()
    {
        var trip = new Trip
        {
            Destination = "Lisboa",
            Country = "Portugal",
            StartDate = new DateOnly(2026, 8, 1),
            EndDate = new DateOnly(2026, 8, 10),
            NumberOfPeople = 2,
            Budget = 10000m,
            Objectives = "Turismo e gastronomia"
        };

        var prompt = PromptBuilder.Build(trip);

        Assert.Contains("Lisboa", prompt);
        Assert.Contains("Portugal", prompt);
    }

    [Fact]
    public void ShouldCalculateDaysCorrectly()
    {
        var trip = new Trip
        {
            Destination = "Paris",
            Country = "França",
            StartDate = new DateOnly(2026, 7, 1),
            EndDate = new DateOnly(2026, 7, 10),
            NumberOfPeople = 1,
            Budget = 5000m,
            Objectives = "Lazer"
        };

        var prompt = PromptBuilder.Build(trip);

        Assert.Contains("10 dias", prompt);
    }

    [Fact]
    public void ShouldIncludeBudgetFormatted()
    {
        var trip = new Trip
        {
            Destination = "Nova York",
            Country = "EUA",
            StartDate = new DateOnly(2026, 12, 1),
            EndDate = new DateOnly(2026, 12, 15),
            NumberOfPeople = 1,
            Budget = 15000m,
            Objectives = "Compras"
        };

        var prompt = PromptBuilder.Build(trip);
        var expected = 15000m.ToString("C", CultureInfo.CurrentCulture);

        Assert.Contains(expected, prompt);
    }

    [Fact]
    public void ShouldIncludeObjectives()
    {
        var trip = new Trip
        {
            Destination = "Roma",
            Country = "Itália",
            StartDate = new DateOnly(2026, 9, 5),
            EndDate = new DateOnly(2026, 9, 12),
            NumberOfPeople = 2,
            Budget = 8000m,
            Objectives = "Conhecer pontos turísticos e gastronomia"
        };

        var prompt = PromptBuilder.Build(trip);

        Assert.Contains("Conhecer pontos turísticos e gastronomia", prompt);
    }

    [Fact]
    public void ShouldIncludeAdditionalNotesWhenProvided()
    {
        var trip = new Trip
        {
            Destination = "Tóquio",
            Country = "Japão",
            StartDate = new DateOnly(2026, 4, 1),
            EndDate = new DateOnly(2026, 4, 15),
            NumberOfPeople = 1,
            Budget = 20000m,
            Objectives = "Cultura",
            AdditionalNotes = "Alergia a frutos do mar"
        };

        var prompt = PromptBuilder.Build(trip);

        Assert.Contains("Alergia a frutos do mar", prompt);
        Assert.Contains("Notas Personalizadas", prompt);
    }

    [Fact]
    public void ShouldNotIncludeAdditionalNotesWhenEmpty()
    {
        var trip = new Trip
        {
            Destination = "Berlim",
            Country = "Alemanha",
            StartDate = new DateOnly(2026, 6, 1),
            EndDate = new DateOnly(2026, 6, 5),
            NumberOfPeople = 2,
            Budget = 6000m,
            Objectives = "História"
        };

        var prompt = PromptBuilder.Build(trip);

        Assert.DoesNotContain("Notas Personalizadas", prompt);
    }

    [Fact]
    public void ShouldIncludeRequiredSections()
    {
        var trip = new Trip
        {
            Destination = "Londres",
            Country = "Reino Unido",
            StartDate = new DateOnly(2026, 5, 1),
            EndDate = new DateOnly(2026, 5, 7),
            NumberOfPeople = 1,
            Budget = 7000m,
            Objectives = "Visitar museus"
        };

        var prompt = PromptBuilder.Build(trip);

        Assert.Contains("Planejamento Estratégico", prompt);
        Assert.Contains("Itinerário Detalhado", prompt);
        Assert.Contains("Distribuição Estimada do Orçamento", prompt);
        Assert.Contains("Checklist e Cuidados", prompt);
        Assert.Contains("RESTRIÇÃO", prompt);
    }
}
