using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Repositories;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(long id);
    Task<IEnumerable<Address>> GetByUserIdAsync(long userId);
    Task<IEnumerable<Address>> GetByCityAndStateAsync(string city, string state);
    Task<Address> CreateAsync(Address address);
    Task DeleteAsync(Address address);
}

