using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;

namespace api.Services
{
    public interface IUserService
    {
        Task<JwtResponse?> LoginUserAsync(LoginDto dto);
        Task<UserReadOnlyDTO> RegisterUserAsync(UserInsertDTO dto);
        Task<UserReadOnlyDTO> UpdateUserAsync(UserUpdateDTO dto, int id);
        Task<UserReadOnlyDTO> DeleteUserAsync(int id);
        Task<List<UserReadOnlyDTO>> GetAllUsersAsync();
        Task<UserReadOnlyDTO> GetUserByIdAsync(int id);
        Task<UserReadOnlyDTO> GetUserByUsernameAsync(string username);
        Task<UserReadOnlyDTO> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string username, string email);
    }
}