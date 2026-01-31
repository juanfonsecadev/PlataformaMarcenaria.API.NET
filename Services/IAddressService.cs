using PlataformaMarcenaria.API.DTOs.Address;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Services;

public interface IAddressService
{
    Task<AddressResponseDTO> CreateAddressAsync(AddressCreateDTO createDTO);
    Task<AddressResponseDTO> GetAddressByIdAsync(long id);
    Task<Address> FindAddressByIdAsync(long id);
    Task<IEnumerable<AddressResponseDTO>> GetAddressesByUserIdAsync(long userId);
    Task<IEnumerable<AddressResponseDTO>> GetAddressesByCityAndStateAsync(string city, string state);
    Task DeleteAddressAsync(long id);
}

