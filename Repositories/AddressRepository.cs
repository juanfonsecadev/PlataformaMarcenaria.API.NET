using Microsoft.EntityFrameworkCore;
using PlataformaMarcenaria.API.Data;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly ApplicationDbContext _context;

    public AddressRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Address?> GetByIdAsync(long id)
    {
        return await _context.Addresses
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Address>> GetByUserIdAsync(long userId)
    {
        return await _context.Addresses
            .Where(a => a.UserId == userId)
            .Include(a => a.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Address>> GetByCityAndStateAsync(string city, string state)
    {
        return await _context.Addresses
            .Where(a => a.City == city && a.State == state)
            .Include(a => a.User)
            .ToListAsync();
    }

    public async Task<Address> CreateAsync(Address address)
    {
        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();
        return address;
    }

    public async Task DeleteAsync(Address address)
    {
        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
    }
}

