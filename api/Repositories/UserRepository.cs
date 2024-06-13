using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Security;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class UserRepository : IUserRepositoy
    {

        private readonly AppDbContext? _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            var userToAdd = await _context!.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return userToAdd.Entity;
        }

        public async Task<User?> DeleteUserAsync(int id)
        {
            var userToDelete = await _context?.Users.FirstOrDefaultAsync(x => x.Id == id)!;
            if (userToDelete == null) return null;
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
            return userToDelete;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context?.Users.ToListAsync()!;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _context?.Users.FirstOrDefaultAsync(x => x.Email == email)!;
            if (user == null) return null;
            return user;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var user = await _context?.Users.FirstOrDefaultAsync(x => x.Username == username)!;
            if (user == null) return null;
            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _context?.Users.FirstOrDefaultAsync(x => x.Id == id)!;
            if (user == null) return null;
            return user;
        }

        public async Task<User?> UpdateUserAsync(User user, int id)
        {
            var existingUser = await _context!.Users.FindAsync(id);
            if (existingUser == null) return null;

            existingUser.Email = user.Email;
            existingUser.Username = user.Username;
            existingUser.Password = EncryptionUtil.Encrypt(user.Password);

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            var user = await _context?.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email)!;
            return user != null;
        }
    }
}