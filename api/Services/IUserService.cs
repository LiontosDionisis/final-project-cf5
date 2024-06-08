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
        Task<UserReadOnlyDTO> UpdateUserAsync(UserUpdateDTO dto);
        Task<UserReadOnlyDTO> DeleteUserAsync(int id);
        Task<List<UserReadOnlyDTO>> GetAllUsersAsync();
        Task<UserReadOnlyDTO> GetUserByIdAsync(int id);

    }
}