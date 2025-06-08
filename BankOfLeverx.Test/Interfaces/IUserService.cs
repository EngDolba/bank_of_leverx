using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int key);
        Task<User> CreateAsync(UserDTO dto);
        Task<User?> UpdateAsync(int key, UserDTO dto);
        Task<User?> PatchAsync(int key, UserPatchDTO dto);
        Task<bool> DeleteAsync(int key);
        Task<User?> GetByUsernameAsync(string username);

        Task<string> AuthenticateAsync(string username, string password);
        string GenerateJwtToken(User user);
        
    }
}
