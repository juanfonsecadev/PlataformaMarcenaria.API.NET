using BCrypt.Net;
using PlataformaMarcenaria.API.DTOs.User;
using PlataformaMarcenaria.API.Entities;
using PlataformaMarcenaria.API.Exceptions;
using PlataformaMarcenaria.API.Repositories;

namespace PlataformaMarcenaria.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDTO> CreateUserAsync(UserCreateDTO createDTO)
    {
        if (await _userRepository.ExistsByEmailAsync(createDTO.Email))
        {
            throw new BusinessException("Email já cadastrado");
        }

        var user = new User
        {
            Name = createDTO.Name,
            Email = createDTO.Email,
            Phone = createDTO.Phone,
            Password = BCrypt.Net.BCrypt.HashPassword(createDTO.Password),
            UserType = createDTO.UserType,
            Document = createDTO.Document,
            Active = true,
            Rating = 0.0
        };

        var savedUser = await _userRepository.CreateAsync(user);
        return ConvertToResponseDTO(savedUser);
    }

    public async Task<UserResponseDTO> GetUserByIdAsync(long id)
    {
        var user = await FindUserByIdAsync(id);
        return ConvertToResponseDTO(user);
    }

    public async Task<User> FindUserByIdAsync(long id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new ResourceNotFoundException("Usuário", "id", id);
        }
        return user;
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(ConvertToResponseDTO);
    }

    public async Task<IEnumerable<UserResponseDTO>> GetUsersByTypeAsync(UserType userType)
    {
        var users = await _userRepository.GetByUserTypeAsync(userType);
        return users.Select(ConvertToResponseDTO);
    }

    public async Task<UserResponseDTO> UpdateUserAsync(long id, UserUpdateDTO updateDTO)
    {
        var user = await FindUserByIdAsync(id);

        if (updateDTO.Email != null && updateDTO.Email != user.Email &&
            await _userRepository.ExistsByEmailAsync(updateDTO.Email))
        {
            throw new BusinessException("Email já cadastrado");
        }

        if (updateDTO.Name != null)
            user.Name = updateDTO.Name;
        if (updateDTO.Email != null)
            user.Email = updateDTO.Email;
        if (updateDTO.Phone != null)
            user.Phone = updateDTO.Phone;
        if (updateDTO.Avatar != null)
            user.Avatar = updateDTO.Avatar;
        if (updateDTO.Document != null)
            user.Document = updateDTO.Document;
        if (updateDTO.Active.HasValue)
            user.Active = updateDTO.Active.Value;

        var updatedUser = await _userRepository.UpdateAsync(user);
        return ConvertToResponseDTO(updatedUser);
    }

    public async Task DeleteUserAsync(long id)
    {
        var user = await FindUserByIdAsync(id);
        user.Active = false;
        await _userRepository.UpdateAsync(user);
    }

    private UserResponseDTO ConvertToResponseDTO(User user)
    {
        return new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            UserType = user.UserType,
            Avatar = user.Avatar,
            Document = user.Document,
            Active = user.Active,
            Rating = user.Rating,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}

