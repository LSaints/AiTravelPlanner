using System.ComponentModel.DataAnnotations;
using ATP.Web.Features.Trips.Application;
using ATP.Web.Features.Trips.UI;

namespace ATP.Web.Tests.Features.Trips.UI;

public class TripFormModelTests
{
    private static bool ValidateProperty(TripFormModel model, string propertyName, out List<ValidationResult> results)
    {
        var propInfo = typeof(TripFormModel).GetProperty(propertyName)!;
        var value = propInfo.GetValue(model);
        results = [];
        var context = new ValidationContext(model) { MemberName = propertyName };
        return Validator.TryValidateProperty(value, context, results);
    }

    private static bool ValidateStep(TripFormModel model, int step, out List<ValidationResult> results)
    {
        var fields = step switch
        {
            0 => new[] { nameof(TripFormModel.Destination), nameof(TripFormModel.Country),
                         nameof(TripFormModel.StartDate), nameof(TripFormModel.EndDate) },
            1 => new[] { nameof(TripFormModel.NumberOfPeople), nameof(TripFormModel.Budget) },
            2 => new[] { nameof(TripFormModel.Objectives) },
            _ => []
        };

        results = [];
        var isValid = true;

        foreach (var field in fields)
        {
            var propInfo = typeof(TripFormModel).GetProperty(field)!;
            var value = propInfo.GetValue(model);
            var fieldResults = new List<ValidationResult>();
            var context = new ValidationContext(model) { MemberName = field };

            if (!Validator.TryValidateProperty(value, context, fieldResults))
            {
                isValid = false;
                results.AddRange(fieldResults);
            }
        }

        return isValid;
    }

    // ── Step 0: Destino ──

    [Fact]
    public void ShouldPassValidationWhenDestinationStepIsValid()
    {
        var model = new TripFormModel
        {
            Destination = "Paris",
            Country = "França",
            StartDate = new DateOnly(2026, 7, 10),
            EndDate = new DateOnly(2026, 7, 20)
        };

        var isValid = ValidateStep(model, 0, out var results);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void ShouldFailValidationWhenDestinationIsMissing()
    {
        var model = new TripFormModel
        {
            Destination = "",
            Country = "França",
            StartDate = new DateOnly(2026, 7, 10),
            EndDate = new DateOnly(2026, 7, 20)
        };

        var isValid = ValidateProperty(model, nameof(TripFormModel.Destination), out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.ErrorMessage == "Destino é obrigatório");
    }

    [Fact]
    public void ShouldFailValidationWhenCountryIsMissing()
    {
        var model = new TripFormModel
        {
            Destination = "Paris",
            Country = "",
            StartDate = new DateOnly(2026, 7, 10),
            EndDate = new DateOnly(2026, 7, 20)
        };

        var isValid = ValidateProperty(model, nameof(TripFormModel.Country), out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.ErrorMessage == "País é obrigatório");
    }

    [Fact]
    public void ShouldFailAdvanceWhenDestinationStepHasMissingFields()
    {
        var model = new TripFormModel
        {
            Destination = "",
            Country = "",
            StartDate = new DateOnly(2026, 7, 10),
            EndDate = new DateOnly(2026, 7, 20)
        };

        var isValid = ValidateStep(model, 0, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Any(m => m == nameof(TripFormModel.Destination)));
        Assert.Contains(results, r => r.MemberNames.Any(m => m == nameof(TripFormModel.Country)));
    }

    // ── Step 1: Orçamento ──

    [Fact]
    public void ShouldPassValidationWhenBudgetStepIsValid()
    {
        var model = new TripFormModel
        {
            NumberOfPeople = 2,
            Budget = 5000m
        };

        var isValid = ValidateStep(model, 1, out var results);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void ShouldFailValidationWhenNumberOfPeopleIsZero()
    {
        var model = new TripFormModel
        {
            NumberOfPeople = 0,
            Budget = 5000m
        };

        var isValid = ValidateProperty(model, nameof(TripFormModel.NumberOfPeople), out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.ErrorMessage == "Deve haver pelo menos 1 pessoa");
    }

    [Fact]
    public void ShouldFailValidationWhenBudgetIsZero()
    {
        var model = new TripFormModel
        {
            NumberOfPeople = 2,
            Budget = 0m
        };

        var isValid = ValidateProperty(model, nameof(TripFormModel.Budget), out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.ErrorMessage == "Orçamento deve ser maior que zero");
    }

    [Fact]
    public void ShouldFailAdvanceWhenBudgetStepIsInvalid()
    {
        var model = new TripFormModel
        {
            NumberOfPeople = 0,
            Budget = 0m
        };

        var isValid = ValidateStep(model, 1, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Any(m => m == nameof(TripFormModel.NumberOfPeople)));
        Assert.Contains(results, r => r.MemberNames.Any(m => m == nameof(TripFormModel.Budget)));
    }

    // ── Step 2: Detalhes ──

    [Fact]
    public void ShouldPassValidationWhenDetailsStepIsValid()
    {
        var model = new TripFormModel
        {
            Objectives = "Turismo e compras"
        };

        var isValid = ValidateStep(model, 2, out var results);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void ShouldFailValidationWhenObjectivesIsMissing()
    {
        var model = new TripFormModel
        {
            Objectives = ""
        };

        var isValid = ValidateProperty(model, nameof(TripFormModel.Objectives), out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.ErrorMessage == "Objetivo é obrigatório");
    }

    // ── Data preservation across steps ──

    [Fact]
    public void ShouldPreserveDataWhenNavigatingBetweenSteps()
    {
        var model = new TripFormModel
        {
            Destination = "Paris",
            Country = "França",
            StartDate = new DateOnly(2026, 7, 10),
            EndDate = new DateOnly(2026, 7, 20),
            NumberOfPeople = 3,
            Budget = 8000m,
            Objectives = "Turismo",
            AdditionalNotes = "Preferência por hotéis"
        };

        Assert.Equal("Paris", model.Destination);
        Assert.Equal("França", model.Country);
        Assert.Equal(3, model.NumberOfPeople);
        Assert.Equal(8000m, model.Budget);
        Assert.Equal("Turismo", model.Objectives);
        Assert.Equal("Preferência por hotéis", model.AdditionalNotes);

        ValidateStep(model, 0, out _);
        Assert.Equal("Paris", model.Destination);

        ValidateStep(model, 1, out _);
        Assert.Equal(3, model.NumberOfPeople);

        ValidateStep(model, 2, out _);
        Assert.Equal("Turismo", model.Objectives);
    }

    // ── Model to request mapping ──

    [Fact]
    public void ShouldMapToCreateTripRequestCorrectly()
    {
        var model = new TripFormModel
        {
            Destination = "Londres",
            Country = "Reino Unido",
            StartDate = new DateOnly(2026, 8, 1),
            EndDate = new DateOnly(2026, 8, 10),
            NumberOfPeople = 2,
            Budget = 15000m,
            Objectives = "Negócios",
            AdditionalNotes = "Hotel próximo ao escritório"
        };

        var request = new CreateTripRequest(
            model.Destination,
            model.Country,
            model.StartDate,
            model.EndDate,
            model.NumberOfPeople,
            model.Budget,
            model.Objectives,
            model.AdditionalNotes
        );

        Assert.Equal(model.Destination, request.Destination);
        Assert.Equal(model.Country, request.Country);
        Assert.Equal(model.StartDate, request.StartDate);
        Assert.Equal(model.EndDate, request.EndDate);
        Assert.Equal(model.NumberOfPeople, request.NumberOfPeople);
        Assert.Equal(model.Budget, request.Budget);
        Assert.Equal(model.Objectives, request.Objectives);
        Assert.Equal(model.AdditionalNotes, request.AdditionalNotes);
    }

    // ── Full multi-step flow simulation ──

    [Fact]
    public void ShouldCompleteFullMultiStepFlowSuccessfully()
    {
        var model = new TripFormModel();

        // Step 0: Fill destination data
        model.Destination = "Paris";
        model.Country = "França";
        model.StartDate = new DateOnly(2026, 7, 10);
        model.EndDate = new DateOnly(2026, 7, 20);

        var step0Valid = ValidateStep(model, 0, out _);
        Assert.True(step0Valid);

        // Step 1: Fill budget data
        model.NumberOfPeople = 2;
        model.Budget = 5000m;

        var step1Valid = ValidateStep(model, 1, out _);
        Assert.True(step1Valid);

        // Step 2: Fill details data
        model.Objectives = "Turismo";

        var step2Valid = ValidateStep(model, 2, out _);
        Assert.True(step2Valid);

        // Final: validate all fields and map to request
        var request = new CreateTripRequest(
            model.Destination, model.Country,
            model.StartDate, model.EndDate,
            model.NumberOfPeople, model.Budget,
            model.Objectives, model.AdditionalNotes
        );

        Assert.Equal("Paris", request.Destination);
        Assert.Equal("França", request.Country);
        Assert.Equal(2, request.NumberOfPeople);
        Assert.Equal(5000m, request.Budget);
        Assert.Equal("Turismo", request.Objectives);
    }
}
