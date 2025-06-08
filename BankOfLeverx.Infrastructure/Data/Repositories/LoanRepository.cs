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
            var sql = @"SELECT [Key], Amount, InitialAmount, StartDate, EndDate, Rate, Type, BankerKey, AccountKey 
                        FROM lnm.loans";
            return await _dbConnection.QueryAsync<Loan>(sql);
        }

        public async Task<Loan?> GetByIdAsync(int key)
        {
            var sql = @"SELECT [Key], Amount, InitialAmount, StartDate, EndDate, Rate, Type, BankerKey, AccountKey
                        FROM lnm.loans WHERE [Key] = @key";
            return await _dbConnection.QueryFirstOrDefaultAsync<Loan>(sql, new { key });
        }

        public async Task<Loan> CreateAsync(Loan loan)
        {
            var getKeySql = "SELECT NEXT VALUE FOR lnm.Loan_seq";
            var newKey = await _dbConnection.ExecuteScalarAsync<int>(getKeySql);
            var insertSql = @"
                INSERT INTO lnm.loans (
                    [Key], Amount, InitialAmount, StartDate, EndDate, 
                    Rate, Type, BankerKey, AccountKey,
                    createdAt, createdBy, updatedAt, updatedBy
                )
                VALUES (
                    @Key, @Amount, @InitialAmount, @StartDate, @EndDate, 
                    @Rate, @Type, @BankerKey, @AccountKey,
                    SYSUTCDATETIME(), SYSTEM_USER, SYSUTCDATETIME(), SYSTEM_USER
                )";

            await _dbConnection.ExecuteAsync(insertSql, new
            {
                Key = newKey,
                Amount = loan.Amount,
                InitialAmount = loan.InitialAmount,
                StartDate = loan.StartDate,
                EndDate = loan.EndDate,
                Rate = loan.Rate,
                Type = loan.Type,
                BankerKey = loan.BankerKey,
                AccountKey = loan.AccountKey
            });

            loan.Key = newKey;
            return loan;
        }

        public async Task<Loan?> UpdateAsync(Loan loan)
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
                    AccountKey = @AccountKey,
                    updatedAt = SYSUTCDATETIME(),
                    updatedBy = SYSTEM_USER
                WHERE [Key] = @Key";

            var affectedRows = await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = loan.Key,
                Amount = loan.Amount,
                InitialAmount = loan.InitialAmount,
                StartDate = loan.StartDate,
                EndDate = loan.EndDate,
                Rate = loan.Rate,
                Type = loan.Type,
                BankerKey = loan.BankerKey,
                AccountKey = loan.AccountKey
            });

            return affectedRows > 0 ? await GetByIdAsync(loan.Key) : null;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var deleteSql = "DELETE FROM lnm.loans WHERE [Key] = @Key";
            var affectedRows = await _dbConnection.ExecuteAsync(deleteSql, new { Key = key });
            return affectedRows > 0;
        }
    }
}