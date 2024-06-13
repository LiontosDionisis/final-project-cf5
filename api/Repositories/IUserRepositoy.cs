using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Repositories
{
    public interface IUserRepositoy
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<User> AddUserAsync(User user);
        Task<User?> DeleteUserAsync(int id);
        Task<User?> UpdateUserAsync(User user, int id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> UserExistsAsync(string username, string email);
    }
}