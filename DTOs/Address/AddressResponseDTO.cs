using PlataformaMarcenaria.API.DTOs.User;

namespace PlataformaMarcenaria.API.DTOs.Address;

public class AddressResponseDTO
{
    public long Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string? Complement { get; set; }
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public UserResponseDTO? User { get; set; }

    public string GetFullAddress()
    {
        var sb = new System.Text.StringBuilder()
            .Append(Street)
            .Append(", ")
            .Append(Number);

        if (!string.IsNullOrEmpty(Complement))
        {
            sb.Append(" - ").Append(Complement);
        }

        sb.Append(", ")
            .Append(Neighborhood)
            .Append(", ")
            .Append(City)
            .Append(" - ")
            .Append(State)
            .Append(", ")
            .Append(ZipCode);

        return sb.ToString();
    }
}

