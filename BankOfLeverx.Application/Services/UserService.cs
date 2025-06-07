using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankOfLeverx.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<User?> GetByIdAsync(int key)
        {
            return _repository.GetByIdAsync(key);
        }

        public async Task<User> CreateAsync(UserDTO dto)
        {
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                Key = 0,
                Username = dto.Username,
                Role = dto.Role,
                HashedPassword = hasher.HashPassword(null!, dto.HashedPassword)
            };

            return await _repository.CreateAsync(user);
        }

        public async Task<User?> UpdateAsync(int key, UserDTO dto)
        {
            var user = new User
            {
                Key = key,
                Username = dto.Username,
                HashedPassword = dto.HashedPassword,
                Role = dto.Role
            };

            var updatedUser = await _repository.UpdateAsync(user);
            if (updatedUser is null)
            {
                throw new KeyNotFoundException();
            }

            return updatedUser;
        }

        public async Task<User?> PatchAsync(int key, UserPatchDTO dto)
        {
            var user = await _repository.GetByIdAsync(key);
            if (user is null)
                throw new KeyNotFoundException($"User with Key {key} not found.");

            if (dto.Username is not null)
                user.Username = dto.Username;
            if (dto.HashedPassword is not null)
                user.HashedPassword = dto.HashedPassword;
            if (dto.Role is not null)
                user.Role = dto.Role.Value;

            return await _repository.UpdateAsync(user);
        }

        public Task<bool> DeleteAsync(int key)
        {
            return _repository.DeleteAsync(key);
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            return _repository.GetByUsernameAsync(username);
        }

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("userId", user.Key.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiresInMinutes"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
