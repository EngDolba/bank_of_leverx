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

        public async Task<Customer> CreateAsync(Customer customer)
        {
            var getKeySql = "SELECT NEXT VALUE FOR krn.Customer_seq";
            var newKey = await _dbConnection.ExecuteScalarAsync<int>(getKeySql);

            var insertSql = @"
                INSERT INTO krn.customers (
                    [Key], Name, Surname, Category, Branch,
                    createdAt, createdBy, updatedAt, updatedBy
                )
                VALUES (
                    @Key, @Name, @Surname, @Category, @Branch,
                    SYSUTCDATETIME(), SYSTEM_USER, SYSUTCDATETIME(), SYSTEM_USER
                )";

            await _dbConnection.ExecuteAsync(insertSql, new
            {
                Key = newKey,
                Name = customer.Name,
                Surname = customer.Surname,
                Category = customer.Category,
                Branch = customer.Branch
            });

            customer.Key = newKey;
            return customer;
        }

        public async Task<Customer?> UpdateAsync(Customer customer)
        {
            var updateSql = @"
                UPDATE krn.customers SET
                    Name = @Name,
                    Surname = @Surname,
                    Category = @Category,
                    Branch = @Branch,
                    updatedAt = SYSUTCDATETIME(),
                    updatedBy = SYSTEM_USER
                WHERE [Key] = @Key";

            var affectedRows = await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = customer.Key,
                Name = customer.Name,
                Surname = customer.Surname,
                Category = customer.Category,
                Branch = customer.Branch
            });

            return affectedRows > 0 ? await GetByIdAsync((int)customer.Key) : null;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var deleteSql = "DELETE FROM krn.customers WHERE [Key] = @Key";
            var affectedRows = await _dbConnection.ExecuteAsync(deleteSql, new { Key = key });
            return affectedRows > 0;
        }
    }
}