using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int key);
        Task<User> CreateAsync(User User);
        Task<User?> UpdateAsync(User User);
        Task<bool> DeleteAsync(int key);
        Task<User?> GetByUsernameAsync(string username);
    }
}