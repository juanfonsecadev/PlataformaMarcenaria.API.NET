using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaMarcenaria.API.DTOs.Auth;
using PlataformaMarcenaria.API.DTOs.User;
using PlataformaMarcenaria.API.Repositories;
using PlataformaMarcenaria.API.Security;
using PlataformaMarcenaria.API.Services;
using BCrypt.Net;

namespace PlataformaMarcenaria.API.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenProvider _tokenProvider;

    public AuthenticationController(
        IUserService userService,
        IUserRepository userRepository,
        JwtTokenProvider tokenProvider)
    {
        _userService = userService;
        _userRepository = userRepository;
        _tokenProvider = tokenProvider;
    }

    [HttpPost("login")]
    public async Task<ActionResult<JwtAuthenticationResponse>> Login([FromBody] UserLoginDTO userLoginDTO)
    {
        var user = await _userRepository.GetByEmailAsync(userLoginDTO.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.Password))
        {
            return Unauthorized(new { message = "Email ou senha inválidos" });
        }

        var jwt = _tokenProvider.GenerateToken(user.Email);
        return Ok(new JwtAuthenticationResponse(jwt));
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponseDTO>> Register([FromBody] UserCreateDTO userCreateDTO)
    {
        var user = await _userService.CreateUserAsync(userCreateDTO);
        return CreatedAtAction(nameof(Register), user);
    }
}

