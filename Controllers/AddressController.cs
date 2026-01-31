using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaMarcenaria.API.DTOs.Address;
using PlataformaMarcenaria.API.Services;

namespace PlataformaMarcenaria.API.Controllers;

[ApiController]
[Route("api/addresses")]
[Authorize]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpPost]
    public async Task<ActionResult<AddressResponseDTO>> CreateAddress([FromBody] AddressCreateDTO createDTO)
    {
        var address = await _addressService.CreateAddressAsync(createDTO);
        return CreatedAtAction(nameof(GetAddressById), new { id = address.Id }, address);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AddressResponseDTO>> GetAddressById(long id)
    {
        var address = await _addressService.GetAddressByIdAsync(id);
        return Ok(address);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<AddressResponseDTO>>> GetAddressesByUserId(long userId)
    {
        var addresses = await _addressService.GetAddressesByUserIdAsync(userId);
        return Ok(addresses);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<AddressResponseDTO>>> GetAddressesByCityAndState(
        [FromQuery] string city,
        [FromQuery] string state)
    {
        var addresses = await _addressService.GetAddressesByCityAndStateAsync(city, state);
        return Ok(addresses);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(long id)
    {
        await _addressService.DeleteAddressAsync(id);
        return NoContent();
    }
}

