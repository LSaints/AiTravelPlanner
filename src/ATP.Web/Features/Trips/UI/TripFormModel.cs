using System.ComponentModel.DataAnnotations;

namespace ATP.Web.Features.Trips.UI;

public class TripFormModel
{
    [Required(ErrorMessage = "Destino é obrigatório")]
    public string Destination { get; set; } = string.Empty;

    [Required(ErrorMessage = "País é obrigatório")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de início é obrigatória")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required(ErrorMessage = "Data de término é obrigatória")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Range(1, int.MaxValue, ErrorMessage = "Deve haver pelo menos 1 pessoa")]
    public int NumberOfPeople { get; set; } = 1;

    [Range(0.01, double.MaxValue, ErrorMessage = "Orçamento deve ser maior que zero")]
    public decimal Budget { get; set; }

    [Required(ErrorMessage = "Objetivo é obrigatório")]
    public string Objectives { get; set; } = string.Empty;

    public string AdditionalNotes { get; set; } = string.Empty;
}
