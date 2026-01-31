using PlataformaMarcenaria.API.DTOs.Address;
using PlataformaMarcenaria.API.Entities;
using PlataformaMarcenaria.API.Exceptions;
using PlataformaMarcenaria.API.Repositories;

namespace PlataformaMarcenaria.API.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserService _userService;

    public AddressService(IAddressRepository addressRepository, IUserService userService)
    {
        _addressRepository = addressRepository;
        _userService = userService;
    }

    public async Task<AddressResponseDTO> CreateAddressAsync(AddressCreateDTO createDTO)
    {
        var user = await _userService.FindUserByIdAsync(createDTO.UserId);

        var address = new Address
        {
            Street = createDTO.Street,
            Number = createDTO.Number,
            Complement = createDTO.Complement,
            Neighborhood = createDTO.Neighborhood,
            City = createDTO.City,
            State = createDTO.State,
            ZipCode = createDTO.ZipCode,
            Reference = createDTO.Reference,
            UserId = createDTO.UserId,
            User = user
        };

        var savedAddress = await _addressRepository.CreateAsync(address);
        return await ConvertToResponseDTOAsync(savedAddress);
    }

    public async Task<AddressResponseDTO> GetAddressByIdAsync(long id)
    {
        var address = await FindAddressByIdAsync(id);
        return await ConvertToResponseDTOAsync(address);
    }

    public async Task<Address> FindAddressByIdAsync(long id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null)
        {
            throw new ResourceNotFoundException("Endereço", "id", id);
        }
        return address;
    }

    public async Task<IEnumerable<AddressResponseDTO>> GetAddressesByUserIdAsync(long userId)
    {
        var addresses = await _addressRepository.GetByUserIdAsync(userId);
        var result = new List<AddressResponseDTO>();
        foreach (var address in addresses)
        {
            result.Add(await ConvertToResponseDTOAsync(address));
        }
        return result;
    }

    public async Task<IEnumerable<AddressResponseDTO>> GetAddressesByCityAndStateAsync(string city, string state)
    {
        var addresses = await _addressRepository.GetByCityAndStateAsync(city, state);
        var result = new List<AddressResponseDTO>();
        foreach (var address in addresses)
        {
            result.Add(await ConvertToResponseDTOAsync(address));
        }
        return result;
    }

    public async Task DeleteAddressAsync(long id)
    {
        var address = await FindAddressByIdAsync(id);
        await _addressRepository.DeleteAsync(address);
    }

    private async Task<AddressResponseDTO> ConvertToResponseDTOAsync(Address address)
    {
        var dto = new AddressResponseDTO
        {
            Id = address.Id,
            Street = address.Street,
            Number = address.Number,
            Complement = address.Complement,
            Neighborhood = address.Neighborhood,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode,
            Reference = address.Reference
        };

        if (address.User != null)
        {
            dto.User = await _userService.GetUserByIdAsync(address.User.Id);
        }

        return dto;
    }
}

