using BankOfLeverx.Domain.Models;
using Dapper;
using System.Data;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDbConnection _dbConnection;

        public TransactionRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            var sql = @"SELECT [Key], AccountKey, IsDebit, Category, Amount, Date, Comment 
                        FROM krn.transactions";
            return await _dbConnection.QueryAsync<Transaction>(sql);
        }

        public async Task<Transaction?> GetByIdAsync(int key)
        {
            var sql = @"SELECT [Key], AccountKey, IsDebit, Category, Amount, Date, Comment 
                        FROM krn.transactions WHERE [Key] = @key";
            return await _dbConnection.QueryFirstOrDefaultAsync<Transaction>(sql, new { key });
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            var getKeySql = "SELECT NEXT VALUE FOR krn.Transaction_seq";
            var newKey = await _dbConnection.ExecuteScalarAsync<int>(getKeySql);

            var insertSql = @"
                INSERT INTO krn.transactions (
                    [Key], AccountKey, IsDebit, Category, Amount, Date, Comment,
                    createdAt, createdBy, updatedAt, updatedBy
                )
                VALUES (
                    @Key, @AccountKey, @IsDebit, @Category, @Amount, @Date, @Comment,
                    SYSUTCDATETIME(), SYSTEM_USER, SYSUTCDATETIME(), SYSTEM_USER
                )";

            await _dbConnection.ExecuteAsync(insertSql, new
            {
                Key = newKey,
                transaction.AccountKey,
                transaction.IsDebit,
                transaction.Category,
                transaction.Amount,
                transaction.Date,
                transaction.Comment
            });

            transaction.Key = newKey;
            return transaction;
        }

        public async Task<Transaction?> UpdateAsync(Transaction transaction)
        {
            var updateSql = @"
                UPDATE krn.transactions SET
                    AccountKey = @AccountKey,
                    IsDebit = @IsDebit,
                    Category = @Category,
                    Amount = @Amount,
                    Date = @Date,
                    Comment = @Comment,
                    updatedAt = SYSUTCDATETIME(),
                    updatedBy = SYSTEM_USER
                WHERE [Key] = @Key";

            var affectedRows = await _dbConnection.ExecuteAsync(updateSql, transaction);
            return affectedRows > 0 ? transaction : null;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var deleteSql = "DELETE FROM krn.transactions WHERE [Key] = @Key";
            var affectedRows = await _dbConnection.ExecuteAsync(deleteSql, new { Key = key });
            return affectedRows > 0;
        }
    }
}