using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using Dapper;
using System.Data;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly IDbConnection _dbConnection;

        public LoanRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            var sql = @"SELECT [Key], Amount, InitialAmount, StartDate, EndDate, Rate, Type, BankerKey, AccountKey FROM lnm.loans";
            return await _dbConnection.QueryAsync<Loan>(sql);
        }

        public async Task<Loan?> GetByIdAsync(int key)
        {
            var sql = @"SELECT [Key], Amount, InitialAmount, StartDate, EndDate, Rate, Type, BankerKey, AccountKey 
                        FROM lnm.loans WHERE [Key] = @key";
            return await _dbConnection.QueryFirstOrDefaultAsync<Loan>(sql, new { key });
        }

        public async Task<Loan> CreateAsync(LoanDTO loanDto)
        {
            var getKeySql = "SELECT NEXT VALUE FOR hr.Loan_seq";
            var newKey = await _dbConnection.ExecuteScalarAsync<int>(getKeySql);

            var insertSql = @"
                INSERT INTO lnm.loans ([Key], Amount, InitialAmount, StartDate, EndDate, Rate, Type, BankerKey, AccountKey)
                VALUES (@Key, @Amount, @InitialAmount, @StartDate, @EndDate, @Rate, @Type, @BankerKey, @AccountKey)";

            await _dbConnection.ExecuteAsync(insertSql, new
            {
                Key = newKey,
                Amount = loanDto.Amount,
                InitialAmount = loanDto.InitialAmount,
                StartDate = loanDto.StartDate,
                EndDate = loanDto.EndDate,
                Rate = loanDto.Rate,
                Type = loanDto.Type,
                BankerKey = loanDto.BankerKey,
                AccountKey = loanDto.AccountKey
            });

            return new Loan
            {
                Key = newKey,
                Amount = loanDto.Amount,
                InitialAmount = loanDto.InitialAmount,
                StartDate = loanDto.
                StartDate,
                EndDate = loanDto.EndDate,
                Rate = loanDto.Rate,
                Type = loanDto.Type,
                BankerKey = loanDto.BankerKey,
                AccountKey = loanDto.AccountKey
            };
        }

        public async Task<Loan?> UpdateAsync(int key, LoanDTO loanDto)
        {
            var updateSql = @"
                UPDATE lnm.loans SET
                    Amount = @Amount,
                    InitialAmount = @InitialAmount,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    Rate = @Rate,
                    Type = @Type,
                    BankerKey = @BankerKey,
                    AccountKey = @AccountKey
                WHERE [Key] = @Key";

            var affectedRows = await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = key,
                Amount = loanDto.Amount,
                InitialAmount = loanDto.InitialAmount,
                StartDate = loanDto.StartDate,
                EndDate = loanDto.EndDate,
                Rate = loanDto.Rate,
                Type = loanDto.Type,
                BankerKey = loanDto.BankerKey,
                AccountKey = loanDto.AccountKey
            });

            return affectedRows > 0 ? await GetByIdAsync(key) : null;
        }

        public async Task<Loan?> PatchAsync(int key, LoanPatchDTO patch)
        {
            var existing = await GetByIdAsync(key);
            if (existing is null) return null;

            var updateSql = @"
                UPDATE lnm.loans SET
                    Amount = @Amount,
                    InitialAmount = @InitialAmount,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    Rate = @Rate,
                    Type = @Type,
                    BankerKey = @BankerKey,
                    AccountKey = @AccountKey
                WHERE [Key] = @Key";

            await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = key,
                Amount = patch.Amount ?? existing.Amount,
                InitialAmount = patch.InitialAmount ?? existing.InitialAmount,
                StartDate = patch.StartDate ?? existing.StartDate,
                EndDate = patch.EndDate ?? existing.EndDate,
                Rate = patch.Rate ?? existing.Rate,
                Type = patch.Type ?? existing.Type,
                BankerKey = patch.BankerKey ?? existing.BankerKey,
                AccountKey = patch.AccountKey ?? existing.AccountKey
            });

            return await GetByIdAsync(key);
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var deleteSql = "DELETE FROM lnm.loans WHERE [Key] = @Key";
            var affectedRows = await _dbConnection.ExecuteAsync(deleteSql, new { Key = key });
            return affectedRows > 0;
        }
    }
}
