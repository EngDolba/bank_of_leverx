using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankOfLeverx.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(int key);
        Task<Account> CreateAsync(AccountDTO dto);
        Task<Account?> UpdateAsync(int key, AccountDTO dto);
        Task<Account?> PatchAsync(int key, AccountPatchDTO dto);
        Task<bool> DeleteAsync(int key);
    }
}
