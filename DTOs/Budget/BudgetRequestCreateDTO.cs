using System.ComponentModel.DataAnnotations;

namespace PlataformaMarcenaria.API.DTOs.Budget;

public class BudgetRequestCreateDTO
{
    [Required(ErrorMessage = "ID do cliente é obrigatório")]
    public long ClientId { get; set; }

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Description { get; set; } = string.Empty;

    public List<string>? ReferenceImages { get; set; }

    [Required(ErrorMessage = "ID do endereço é obrigatório")]
    public long LocationId { get; set; }

    public decimal? EstimatedBudget { get; set; }

    public DateTime? DesiredDeadline { get; set; }
}

