using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Repositories;

public interface IVisitRepository
{
    Task<Visit?> GetByIdAsync(long id);
    Task<IEnumerable<Visit>> GetBySellerIdAsync(long sellerId);
    Task<IEnumerable<Visit>> GetByBudgetRequestIdAsync(long budgetRequestId);
    Task<IEnumerable<Visit>> GetBySellerIdAndScheduledDateBetweenAsync(long sellerId, DateTime start, DateTime end);
    Task<Visit> CreateAsync(Visit visit);
    Task<Visit> UpdateAsync(Visit visit);
    Task DeleteAsync(Visit visit);
}

