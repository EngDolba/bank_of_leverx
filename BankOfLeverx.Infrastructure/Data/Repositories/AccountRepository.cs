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

        public async Task<Account> CreateAsync(Account account)
        {
            var getKeySql = "SELECT NEXT VALUE FOR krn.Account_seq";
            var newKey = await _dbConnection.ExecuteScalarAsync<int>(getKeySql);

            var insertSql = @"
                INSERT INTO krn.accounts (
                    [Key], Number, PlanCode, Balance, CustomerKey,
                    createdAt, createdBy, updatedAt, updatedBy
                )
                VALUES (
                    @Key, @Number, @PlanCode, @Balance, @CustomerKey,
                    SYSUTCDATETIME(), SYSTEM_USER, SYSUTCDATETIME(), SYSTEM_USER
                )";

            await _dbConnection.ExecuteAsync(insertSql, new
            {
                Key = newKey,
                Number = account.Number,
                PlanCode = account.PlanCode,
                Balance = account.Balance,
                CustomerKey = account.CustomerKey
            });

            account.Key = newKey;
            return account;
        }

        public async Task<Account?> UpdateAsync(Account account)
        {
            var updateSql = @"
                UPDATE krn.accounts SET
                    Number = @Number,
                    PlanCode = @PlanCode,
                    Balance = @Balance,
                    CustomerKey = @CustomerKey,
                    updatedAt = SYSUTCDATETIME(),
                    updatedBy = SYSTEM_USER
                WHERE [Key] = @Key";

            var affectedRows = await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = account.Key,
                Number = account.Number,
                PlanCode = account.PlanCode,
                Balance = account.Balance,
                CustomerKey = account.CustomerKey
            });

            return affectedRows > 0 ? await GetByIdAsync((int)account.Key) : null;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var deleteSql = "DELETE FROM krn.accounts WHERE [Key] = @Key";
            var affectedRows = await _dbConnection.ExecuteAsync(deleteSql, new { Key = key });
            return affectedRows > 0;
        }
    }
}