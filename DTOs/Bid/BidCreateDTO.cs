using System.ComponentModel.DataAnnotations;

namespace PlataformaMarcenaria.API.DTOs.Bid;

public class BidCreateDTO
{
    [Required(ErrorMessage = "ID do marceneiro é obrigatório")]
    public long CarpenterId { get; set; }

    [Required(ErrorMessage = "ID do orçamento é obrigatório")]
    public long BudgetRequestId { get; set; }

    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Prazo de execução é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Prazo deve ser de pelo menos 1 dia")]
    public int ExecutionTimeInDays { get; set; }

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string Description { get; set; } = string.Empty;
}

