using Microsoft.EntityFrameworkCore;
using PlataformaMarcenaria.API.Data;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Repositories;

public class VisitRepository : IVisitRepository
{
    private readonly ApplicationDbContext _context;

    public VisitRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Visit?> GetByIdAsync(long id)
    {
        return await _context.Visits
            .Include(v => v.Seller)
            .Include(v => v.BudgetRequest)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<Visit>> GetBySellerIdAsync(long sellerId)
    {
        return await _context.Visits
            .Where(v => v.SellerId == sellerId)
            .Include(v => v.Seller)
            .Include(v => v.BudgetRequest)
            .ToListAsync();
    }

    public async Task<IEnumerable<Visit>> GetByBudgetRequestIdAsync(long budgetRequestId)
    {
        return await _context.Visits
            .Where(v => v.BudgetRequestId == budgetRequestId)
            .Include(v => v.Seller)
            .Include(v => v.BudgetRequest)
            .ToListAsync();
    }

    public async Task<IEnumerable<Visit>> GetBySellerIdAndScheduledDateBetweenAsync(long sellerId, DateTime start, DateTime end)
    {
        return await _context.Visits
            .Where(v => v.SellerId == sellerId && v.ScheduledDate >= start && v.ScheduledDate <= end)
            .Include(v => v.Seller)
            .Include(v => v.BudgetRequest)
            .ToListAsync();
    }

    public async Task<Visit> CreateAsync(Visit visit)
    {
        _context.Visits.Add(visit);
        await _context.SaveChangesAsync();
        return visit;
    }

    public async Task<Visit> UpdateAsync(Visit visit)
    {
        _context.Visits.Update(visit);
        await _context.SaveChangesAsync();
        return visit;
    }

    public async Task DeleteAsync(Visit visit)
    {
        _context.Visits.Remove(visit);
        await _context.SaveChangesAsync();
    }
}

