using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;
using api.Repositories;
using api.Security;
using api.Services.Exceptions;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepositoy? _userRepo;
        private readonly IMapper? _mapper;
        private readonly IConfiguration? _configuration;

        public UserService(IUserRepositoy userRepo, IMapper mapper, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<UserReadOnlyDTO> DeleteUserAsync(int id)
        {
            var user = await _userRepo!.GetUserByIdAsync(id);
            if (user == null) return null!;

            await _userRepo!.DeleteUserAsync(user.Id);

            var userDeleted = _mapper!.Map<UserReadOnlyDTO>(user);
            return userDeleted;
        }

        public async Task<List<UserReadOnlyDTO>> GetAllUsersAsync()
        {
            var users = await _userRepo!.GetAllAsync();

            var usersToReturn = _mapper!.Map<List<UserReadOnlyDTO>>(users);

            return usersToReturn;
        }

        public async Task<UserReadOnlyDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepo!.GetByEmailAsync(email);
            if (user == null) return null!;

            var userToReturn = _mapper!.Map<UserReadOnlyDTO>(user);
            return userToReturn;
        }

        public async Task<UserReadOnlyDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepo!.GetUserByIdAsync(id);
            if (user == null) return null!;

            var userToReturn = _mapper!.Map<UserReadOnlyDTO>(user);
            return userToReturn; 
        }

        public async Task<UserReadOnlyDTO> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepo!.GetByUsernameAsync(username);
            if (user == null) return null!;
            var userToReturn = _mapper!.Map<UserReadOnlyDTO>(user);
            return userToReturn;
        }

        public async Task<JwtResponse?> LoginUserAsync(LoginDto dto)
        {
            var user = await _userRepo?.GetByUsernameAsync(dto.Username!)!;
            if (user == null || !EncryptionUtil.isValidPassword(dto.Password!, user.Password)) return null;

            // JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration!["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new JwtResponse
            {
                Token = tokenString,
                Expires = token.ValidTo.ToString()
            };
        }

        public async Task<UserReadOnlyDTO> RegisterUserAsync(UserInsertDTO dto)
        {
            var user = ExtractUser(dto);
            User? existingUser = await _userRepo!.GetByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException("User exists " + existingUser.Username);
            }

            user.Password = EncryptionUtil.Encrypt(user.Password);
            await _userRepo!.AddUserAsync(user);
            return _mapper!.Map<UserReadOnlyDTO>(user);
}


        public async Task<UserReadOnlyDTO> UpdateUserAsync(UserUpdateDTO dto, int id)
        {
            var user = await _userRepo!.GetUserByIdAsync(id);
            if (user == null) return null!;

            if(user.Email != null) user.Email = dto.Email!;
            if(user.Username != null) user.Username = dto.Username!;
            if(user.Password != null) user.Password = EncryptionUtil.Encrypt(dto.Password!);

            await _userRepo.UpdateUserAsync(user, user.Id);

            var updatedUser = _mapper!.Map<UserReadOnlyDTO>(user);

            return updatedUser;
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            return await _userRepo!.UserExistsAsync(username, email);
        }

        private User ExtractUser(UserInsertDTO signupDTO)
        {
            return new User()
            {
                Username = signupDTO.Username!,
                Password = signupDTO.Password!,
                Email = signupDTO.Email!,
            };
        }
    }
}