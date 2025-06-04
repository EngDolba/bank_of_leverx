using BankOfLeverx.Domain.Models;
using Dapper;
using System.Data;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbConnection _dbConnection;

        public EmployeeRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var sql = "SELECT [key], name, surname, position, branch FROM hr.employees";
            return await _dbConnection.QueryAsync<Employee>(sql);
        }

        public async Task<Employee?> GetByIdAsync(int key)
        {
            var sql = "SELECT [key], name, surname, position, branch FROM hr.employees WHERE [key] = @key";
            return await _dbConnection.QueryFirstOrDefaultAsync<Employee>(sql, new { key });
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            var getKeySql = "SELECT NEXT VALUE FOR hr.employee_seq";
            var newKey = await _dbConnection.ExecuteScalarAsync<int>(getKeySql);

            var insertSql = @"
                INSERT INTO hr.employees ([key], name, surname, position, branch)
                VALUES (@key, @name, @surname, @position, @branch)";

            await _dbConnection.ExecuteAsync(insertSql, new
            {
                key = newKey,
                name = employee.Name,
                surname = employee.Surname,
                position = employee.Position,
                branch = employee.Branch
            });

            employee.Key = newKey;
            return employee;
        }

        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            var updateSql = @"
                UPDATE hr.employees
                SET name = @name, surname = @surname, position = @position, branch = @branch
                WHERE [key] = @key";

            var affectedRows = await _dbConnection.ExecuteAsync(updateSql, new
            {
                key = employee.Key,
                name = employee.Name,
                surname = employee.Surname,
                position = employee.Position,
                branch = employee.Branch
            });

            return affectedRows > 0 ? await GetByIdAsync(employee.Key) : null;
        }

        public async Task<Employee?> PatchAsync(Employee employeePatch)
        {
            var existing = await GetByIdAsync(employeePatch.Key);
            if (existing is null) return null;

            var updateSql = @"
                UPDATE hr.employees
                SET name = @name,
                    surname = @surname,
                    position = @position,
                    branch = @branch
                WHERE [key] = @key";

            var name = employeePatch.Name ;
            var surname = employeePatch.Surname;
            var position = employeePatch.Position;
            var branch = employeePatch.Branch;

            await _dbConnection.ExecuteAsync(updateSql, new
            {
                key = employeePatch.Key,
                name,
                surname,
                position,
                branch
            });

            return await GetByIdAsync(employeePatch.Key);
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var deleteSql = "DELETE FROM hr.employees WHERE [key] = @key";
            var affectedRows = await _dbConnection.ExecuteAsync(deleteSql, new { key });
            return affectedRows > 0;
        }
    }
}
