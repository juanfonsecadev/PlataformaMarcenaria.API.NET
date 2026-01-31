using System.ComponentModel.DataAnnotations;

namespace PlataformaMarcenaria.API.DTOs.User;

public class UserUpdateDTO
{
    public string? Name { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    public string? Email { get; set; }

    public string? Phone { get; set; }
    public string? Avatar { get; set; }
    public string? Document { get; set; }
    public bool? Active { get; set; }
}

