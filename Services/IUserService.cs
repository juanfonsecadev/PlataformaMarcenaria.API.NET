using PlataformaMarcenaria.API.DTOs.User;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Services;

public interface IUserService
{
    Task<UserResponseDTO> CreateUserAsync(UserCreateDTO createDTO);
    Task<UserResponseDTO> GetUserByIdAsync(long id);
    Task<User> FindUserByIdAsync(long id);
    Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
    Task<IEnumerable<UserResponseDTO>> GetUsersByTypeAsync(UserType userType);
    Task<UserResponseDTO> UpdateUserAsync(long id, UserUpdateDTO updateDTO);
    Task DeleteUserAsync(long id);
}

