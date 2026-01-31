using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlataformaMarcenaria.API.Controllers;

[ApiController]
[Route("api/test")]
[AllowAnonymous]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return Ok(new { message = "API funcionando!" });
    }

    [HttpPost("login")]
    public IActionResult TestLogin([FromBody] Dictionary<string, string> loginData)
    {
        return Ok(new
        {
            message = "Login endpoint funcionando!",
            email = loginData.GetValueOrDefault("email"),
            password = loginData.GetValueOrDefault("password")
        });
    }
}

