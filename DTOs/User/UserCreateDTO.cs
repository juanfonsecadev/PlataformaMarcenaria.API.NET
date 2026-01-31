using System.ComponentModel.DataAnnotations;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.DTOs.User;

public class UserCreateDTO
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo de usuário é obrigatório")]
    public UserType UserType { get; set; }

    public string? Document { get; set; }
}

