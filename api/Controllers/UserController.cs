using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Services;
using api.Services.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService? _service;
        private readonly IMapper _mapper;
        public UserController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _service!.GetAllUsersAsync();
            if (users == null) return NotFound();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var user = await _service!.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserInsertDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            try
            {
                var userExists = await _service!.UserExistsAsync(dto.Username!, dto.Email!);
                if (userExists) return Conflict(new {message = "Username or Email already exists."});

                await _service!.RegisterUserAsync(dto);
                return Ok(dto);
            }
            catch (UserAlreadyExistsException e)
            {
                return Conflict(new {messge = e.Message});
            }
            catch (Exception)
            {
                return StatusCode(500, new {message = "Internal Server Error."});
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var user = await _service!.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found");
            await _service.DeleteUserAsync(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserUpdateDTO dto, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var user = await _service!.GetUserByIdAsync(id);
                if (user == null) return NotFound("User was not found");

                var userExists = await _service!.UserExistsAsync(dto.Username!, dto.Email!);
                if (userExists) return Conflict(new {message = "Username or Email already exists."});

                if (dto.Email != null) user.Email = dto.Email;
                if (dto.Username != null) user.Username = dto.Username;
                if (dto.Password != null) user.Password = dto.Password;
                var updatedUser = _mapper.Map<UserUpdateDTO>(user);
                await _service.UpdateUserAsync(updatedUser, id);
                return Ok(updatedUser);
            }
            catch (Exception e)
            {
                return StatusCode(500, new {message = "Error while updating user" + e.Message});
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDto dto)
        {
            if (dto == null) return BadRequest("Invalid client request");

            var response = await _service!.LoginUserAsync(dto);
            if (response == null) return Unauthorized();
            return Ok(response);
        }
    }
}