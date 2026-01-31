using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Repositories;

public interface IBidRepository
{
    Task<Bid?> GetByIdAsync(long id);
    Task<IEnumerable<Bid>> GetByCarpenterIdAsync(long carpenterId);
    Task<IEnumerable<Bid>> GetByBudgetRequestIdAsync(long budgetRequestId);
    Task<IEnumerable<Bid>> GetByBudgetRequestIdAndStatusAsync(long budgetRequestId, Bid.BidStatus status);
    Task<Bid> CreateAsync(Bid bid);
    Task<Bid> UpdateAsync(Bid bid);
    Task DeleteAsync(Bid bid);
}

