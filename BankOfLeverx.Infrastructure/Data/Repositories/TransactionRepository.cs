using BankOfLeverx.Core.DTO;
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

        public async Task<Transaction> CreateAsync(TransactionDTO transactionDto)
        {
            var getKeySql = "SELECT NEXT VALUE FOR krn.Transaction_seq";
            var newKey = await _dbConnection.ExecuteScalarAsync<int>(getKeySql);

            var insertSql = @"
                INSERT INTO krn.transactions ([Key], AccountKey, IsDebit, Category, Amount, Date, Comment)
                VALUES (@Key, @AccountKey, @IsDebit, @Category, @Amount, @Date, @Comment)";

            await _dbConnection.ExecuteAsync(insertSql, new
            {
                Key = newKey,
                AccountKey = transactionDto.AccountKey,
                IsDebit = transactionDto.IsDebit,
                Category = transactionDto.Category,
                Amount = transactionDto.Amount,
                Date = transactionDto.Date,
                Comment = transactionDto.Comment
            });

            return new Transaction
            {
                Key = newKey,
                AccountKey = transactionDto.AccountKey,
                IsDebit = transactionDto.IsDebit,
                Category = transactionDto.Category,
                Amount = transactionDto.Amount,
                Date = transactionDto.Date,
                Comment = transactionDto.Comment
            };
        }

        public async Task<Transaction?> UpdateAsync(int key, TransactionDTO transactionDto)
        {
            var updateSql = @"
                UPDATE krn.transactions SET
                    AccountKey = @AccountKey,
                    IsDebit = @IsDebit,
                    Category = @Category,
                    Amount = @Amount,
                    Date = @Date,
                    Comment = @Comment
                WHERE [Key] = @Key";

            var affectedRows = await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = key,
                AccountKey = transactionDto.AccountKey,
                IsDebit = transactionDto.IsDebit,
                Category = transactionDto.Category,
                Amount = transactionDto.Amount,
                Date = transactionDto.Date,
                Comment = transactionDto.Comment
            });

            return affectedRows > 0 ? await GetByIdAsync(key) : null;
        }

        public async Task<Transaction?> PatchAsync(int key, TransactionPatchDTO patch)
        {
            var existing = await GetByIdAsync(key);
            if (existing is null) return null;

            var updateSql = @"
                UPDATE krn.transactions SET
                    AccountKey = @AccountKey,
                    IsDebit = @IsDebit,
                    Category = @Category,
                    Amount = @Amount,
                    Date = @Date,
                    Comment = @Comment
                WHERE [Key] = @Key";

            await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = key,
                AccountKey = patch.AccountKey ?? existing.AccountKey,
                IsDebit = patch.IsDebit ?? existing.IsDebit,
                Category = patch.Category ?? existing.Category,
                Amount = patch.Amount ?? existing.Amount,
                Date = patch.Date ?? existing.Date,
                Comment = patch.Comment ?? existing.Comment
            });

            return await GetByIdAsync(key);
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var deleteSql = "DELETE FROM krn.transactions WHERE [Key] = @Key";
            var affectedRows = await _dbConnection.ExecuteAsync(deleteSql, new { Key = key });
            return affectedRows > 0;
        }
    }
}