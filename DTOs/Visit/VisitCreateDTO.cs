using System.ComponentModel.DataAnnotations;

namespace PlataformaMarcenaria.API.DTOs.Visit;

public class VisitCreateDTO
{
    [Required(ErrorMessage = "ID do vendedor é obrigatório")]
    public long SellerId { get; set; }

    [Required(ErrorMessage = "ID do orçamento é obrigatório")]
    public long BudgetRequestId { get; set; }

    [Required(ErrorMessage = "Data agendada é obrigatória")]
    public DateTime ScheduledDate { get; set; }

    public string? Notes { get; set; }
}

