using PlataformaMarcenaria.API.DTOs.Budget;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Services;

public interface IBudgetRequestService
{
    Task<BudgetRequestResponseDTO> CreateBudgetRequestAsync(BudgetRequestCreateDTO createDTO);
    Task<BudgetRequestResponseDTO> GetBudgetRequestByIdAsync(long id);
    Task<BudgetRequest> FindBudgetRequestByIdAsync(long id);
    Task<IEnumerable<BudgetRequestResponseDTO>> GetBudgetRequestsByClientIdAsync(long clientId);
    Task<IEnumerable<BudgetRequestResponseDTO>> GetBudgetRequestsByStatusAsync(BudgetRequest.BudgetStatus status);
    Task<IEnumerable<BudgetRequestResponseDTO>> GetBudgetRequestsByLocationAsync(string city, string state);
    Task<BudgetRequestResponseDTO> UpdateBudgetRequestStatusAsync(long id, BudgetRequest.BudgetStatus status);
    Task DeleteBudgetRequestAsync(long id);
}

