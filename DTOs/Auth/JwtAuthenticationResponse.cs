namespace PlataformaMarcenaria.API.DTOs.Auth;

public class JwtAuthenticationResponse
{
    public string Token { get; set; } = string.Empty;

    public JwtAuthenticationResponse(string token)
    {
        Token = token;
    }
}

