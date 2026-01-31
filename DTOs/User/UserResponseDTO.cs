using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.DTOs.User;

public class UserResponseDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public UserType UserType { get; set; }
    public string? Avatar { get; set; }
    public string? Document { get; set; }
    public bool Active { get; set; }
    public double Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

