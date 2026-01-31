using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaMarcenaria.API.DTOs.User;
using PlataformaMarcenaria.API.Entities;
using PlataformaMarcenaria.API.Services;

namespace PlataformaMarcenaria.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponseDTO>> CreateUser([FromBody] UserCreateDTO createDTO)
    {
        var user = await _userService.CreateUserAsync(createDTO);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserResponseDTO>> GetUserById(long id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsers([FromQuery] UserType? userType)
    {
        if (userType.HasValue)
        {
            var users = await _userService.GetUsersByTypeAsync(userType.Value);
            return Ok(users);
        }
        var allUsers = await _userService.GetAllUsersAsync();
        return Ok(allUsers);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<UserResponseDTO>> UpdateUser(long id, [FromBody] UserUpdateDTO updateDTO)
    {
        var user = await _userService.UpdateUserAsync(id, updateDTO);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(long id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}

