using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Repositories;

public interface IBudgetRequestRepository
{
    Task<BudgetRequest?> GetByIdAsync(long id);
    Task<IEnumerable<BudgetRequest>> GetByClientIdAsync(long clientId);
    Task<IEnumerable<BudgetRequest>> GetByStatusAsync(BudgetRequest.BudgetStatus status);
    Task<IEnumerable<BudgetRequest>> GetByLocationAsync(string city, string state);
    Task<BudgetRequest> CreateAsync(BudgetRequest budgetRequest);
    Task<BudgetRequest> UpdateAsync(BudgetRequest budgetRequest);
    Task DeleteAsync(BudgetRequest budgetRequest);
}

