using System.ComponentModel.DataAnnotations;

namespace PlataformaMarcenaria.API.DTOs.Address;

public class AddressCreateDTO
{
    [Required(ErrorMessage = "Rua é obrigatória")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "Número é obrigatório")]
    public string Number { get; set; } = string.Empty;

    public string? Complement { get; set; }

    [Required(ErrorMessage = "Bairro é obrigatório")]
    public string Neighborhood { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cidade é obrigatória")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Estado é obrigatório")]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "CEP é obrigatório")]
    public string ZipCode { get; set; } = string.Empty;

    public string? Reference { get; set; }

    [Required(ErrorMessage = "ID do usuário é obrigatório")]
    public long UserId { get; set; }
}

