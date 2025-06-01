using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using Dapper;
using System.Data;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _dbConnection;

        public AccountRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            var sql = @"SELECT [Key], Number, PlanCode, Balance, CustomerKey FROM krn.accounts";
            return await _dbConnection.QueryAsync<Account>(sql);
        }

        public async Task<Account?> GetByIdAsync(int key)
        {
            var sql = @"SELECT [Key], Number, PlanCode, Balance, CustomerKey
                        FROM krn.accounts WHERE [Key] = @key";
            return await _dbConnection.QueryFirstOrDefaultAsync<Account>(sql, new { key });
        }

        public async Task<Account> CreateAsync(AccountDTO accountDto)
        {
            var getKeySql = "SELECT NEXT VALUE FOR krn.Account_seq";
            var newKey = await _dbConnection.ExecuteScalarAsync<int>(getKeySql);

            var insertSql = @"
                INSERT INTO krn.accounts ([Key], Number, PlanCode, Balance, CustomerKey)
                VALUES (@Key, @Number, @PlanCode, @Balance, @CustomerKey)";

            await _dbConnection.ExecuteAsync(insertSql, new
            {
                Key = newKey,
                Number = accountDto.Number,
                PlanCode = accountDto.PlanCode,
                Balance = accountDto.Balance,
                CustomerKey = accountDto.CustomerKey,
            });

            return new Account
            {
                Key = newKey,
                Number = accountDto.Number,
                PlanCode = accountDto.PlanCode,
                Balance = accountDto.Balance,
                CustomerKey = accountDto.CustomerKey,
            };
        }

        public async Task<Account?> UpdateAsync(int key, AccountDTO accountDto)
        {
            var updateSql = @"
                UPDATE krn.accounts SET
                    Number = @Number,
                    PlanCode = @PlanCode,
                    Balance = @Balance,
                    CustomerKey = @CustomerKey
                WHERE [Key] = @Key";

            var affectedRows = await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = key,
                Number = accountDto.Number,
                PlanCode = accountDto.PlanCode,
                Balance = accountDto.Balance,
                CustomerKey = accountDto.CustomerKey
            });

            return affectedRows > 0 ? await GetByIdAsync(key) : null;
        }

        public async Task<Account?> PatchAsync(int key, AccountPatchDTO patch)
        {
            var existing = await GetByIdAsync(key);
            if (existing is null) return null;

            var updateSql = @"
                UPDATE krn.accounts SET
                    Number = @Number,
                    PlanCode = @PlanCode,
                    Balance = @Balance,
                    CustomerKey = @CustomerKey
                WHERE [Key] = @Key";

            await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = key,
                Number = patch.Number ?? existing.Number,
                PlanCode = patch.PlanCode ?? existing.PlanCode,
                Balance = patch.Balance ?? existing.Balance,
                CustomerKey = patch.CustomerKey ?? existing.CustomerKey
            });

            return await GetByIdAsync(key);
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var deleteSql = "DELETE FROM krn.accounts WHERE [Key] = @Key";
            var affectedRows = await _dbConnection.ExecuteAsync(deleteSql, new { Key = key });
            return affectedRows > 0;
        }
    }
}