using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using Dapper;
using System.Data;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection _dbConnection;

        public CustomerRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var sql = @"SELECT [Key], Name, Surname, Category, Branch FROM krn.customers";
            return await _dbConnection.QueryAsync<Customer>(sql);
        }

        public async Task<Customer?> GetByIdAsync(int key)
        {
            var sql = @"SELECT [Key], Name, Surname, Category, Branch 
                        FROM krn.customers WHERE [Key] = @key";
            return await _dbConnection.QueryFirstOrDefaultAsync<Customer>(sql, new { key });
        }

        public async Task<Customer> CreateAsync(CustomerDTO customerDto)
        {
            var getKeySql = "SELECT NEXT VALUE FOR krn.Customer_seq";
            var newKey = await _dbConnection.ExecuteScalarAsync<int>(getKeySql);

            var insertSql = @"
                INSERT INTO krn.customers ([Key], Name, Surname, Category, Branch)
                VALUES (@Key, @Name, @Surname, @Category, @Branch)";

            await _dbConnection.ExecuteAsync(insertSql, new
            {
                Key = newKey,
                Name = customerDto.Name,
                Surname = customerDto.Surname,
                Category = customerDto.Category,
                Branch = customerDto.Branch
            });

            return new Customer
            {
                Key = newKey,
                Name = customerDto.Name,
                Surname = customerDto.Surname,
                Category = customerDto.Category,
                Branch = customerDto.Branch
            };
        }

        public async Task<Customer?> UpdateAsync(int key, CustomerDTO customerDto)
        {
            var updateSql = @"
                UPDATE krn.customers SET
                    Name = @Name,
                    Surname = @Surname,
                    Category = @Category,
                    Branch = @Branch
                WHERE [Key] = @Key";

            var affectedRows = await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = key,
                Name = customerDto.Name,
                Surname = customerDto.Surname,
                Category = customerDto.Category,
                Branch = customerDto.Branch
            });

            return affectedRows > 0 ? await GetByIdAsync(key) : null;
        }

        public async Task<Customer?> PatchAsync(int key, CustomerPatchDTO patch)
        {
            var existing = await GetByIdAsync(key);
            if (existing is null) return null;

            var updateSql = @"
                UPDATE krn.customers SET
                    Name = @Name,
                    Surname = @Surname,
                    Category = @Category,
                    Branch = @Branch
                WHERE [Key] = @Key";

            await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = key,
                Name = patch.Name ?? existing.Name,
                Surname = patch.Surname ?? existing.Surname,
                Category = patch.Category ?? existing.Category,
                Branch = patch.Branch ?? existing.Branch
            });

            return await GetByIdAsync(key);
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var deleteSql = "DELETE FROM krn.customers WHERE [Key] = @Key";
            var affectedRows = await _dbConnection.ExecuteAsync(deleteSql, new { Key = key });
            return affectedRows > 0;
        }
    }
}