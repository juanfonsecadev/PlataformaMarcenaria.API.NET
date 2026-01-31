using Microsoft.EntityFrameworkCore;
using PlataformaMarcenaria.API.Data;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Repositories;

public class BidRepository : IBidRepository
{
    private readonly ApplicationDbContext _context;

    public BidRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Bid?> GetByIdAsync(long id)
    {
        return await _context.Bids
            .Include(b => b.Carpenter)
            .Include(b => b.BudgetRequest)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Bid>> GetByCarpenterIdAsync(long carpenterId)
    {
        return await _context.Bids
            .Where(b => b.CarpenterId == carpenterId)
            .Include(b => b.Carpenter)
            .Include(b => b.BudgetRequest)
            .ToListAsync();
    }

    public async Task<IEnumerable<Bid>> GetByBudgetRequestIdAsync(long budgetRequestId)
    {
        return await _context.Bids
            .Where(b => b.BudgetRequestId == budgetRequestId)
            .Include(b => b.Carpenter)
            .Include(b => b.BudgetRequest)
            .ToListAsync();
    }

    public async Task<IEnumerable<Bid>> GetByBudgetRequestIdAndStatusAsync(long budgetRequestId, Bid.BidStatus status)
    {
        return await _context.Bids
            .Where(b => b.BudgetRequestId == budgetRequestId && b.Status == status)
            .Include(b => b.Carpenter)
            .Include(b => b.BudgetRequest)
            .ToListAsync();
    }

    public async Task<Bid> CreateAsync(Bid bid)
    {
        _context.Bids.Add(bid);
        await _context.SaveChangesAsync();
        return bid;
    }

    public async Task<Bid> UpdateAsync(Bid bid)
    {
        _context.Bids.Update(bid);
        await _context.SaveChangesAsync();
        return bid;
    }

    public async Task DeleteAsync(Bid bid)
    {
        _context.Bids.Remove(bid);
        await _context.SaveChangesAsync();
    }
}

