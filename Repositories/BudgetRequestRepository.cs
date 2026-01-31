using Microsoft.EntityFrameworkCore;
using PlataformaMarcenaria.API.Data;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Repositories;

public class BudgetRequestRepository : IBudgetRequestRepository
{
    private readonly ApplicationDbContext _context;

    public BudgetRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BudgetRequest?> GetByIdAsync(long id)
    {
        return await _context.BudgetRequests
            .Include(br => br.Client)
            .Include(br => br.Location)
            .Include(br => br.Bids)
            .Include(br => br.Visits)
            .FirstOrDefaultAsync(br => br.Id == id);
    }

    public async Task<IEnumerable<BudgetRequest>> GetByClientIdAsync(long clientId)
    {
        return await _context.BudgetRequests
            .Where(br => br.ClientId == clientId)
            .Include(br => br.Client)
            .Include(br => br.Location)
            .ToListAsync();
    }

    public async Task<IEnumerable<BudgetRequest>> GetByStatusAsync(BudgetRequest.BudgetStatus status)
    {
        return await _context.BudgetRequests
            .Where(br => br.Status == status)
            .Include(br => br.Client)
            .Include(br => br.Location)
            .ToListAsync();
    }

    public async Task<IEnumerable<BudgetRequest>> GetByLocationAsync(string city, string state)
    {
        return await _context.BudgetRequests
            .Where(br => br.Location.City == city && br.Location.State == state)
            .Include(br => br.Client)
            .Include(br => br.Location)
            .ToListAsync();
    }

    public async Task<BudgetRequest> CreateAsync(BudgetRequest budgetRequest)
    {
        _context.BudgetRequests.Add(budgetRequest);
        await _context.SaveChangesAsync();
        return budgetRequest;
    }

    public async Task<BudgetRequest> UpdateAsync(BudgetRequest budgetRequest)
    {
        _context.BudgetRequests.Update(budgetRequest);
        await _context.SaveChangesAsync();
        return budgetRequest;
    }

    public async Task DeleteAsync(BudgetRequest budgetRequest)
    {
        _context.BudgetRequests.Remove(budgetRequest);
        await _context.SaveChangesAsync();
    }
}

