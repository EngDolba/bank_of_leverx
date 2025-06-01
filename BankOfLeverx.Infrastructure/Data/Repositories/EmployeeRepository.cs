using BankOfLeverx.Core.DTO;
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

        public async Task<Employee> CreateAsync(EmployeeDTO employee)
        {
            var getKeySql = "SELECT NEXT VALUE FOR hr.employee_seq";
            var newKey = await _dbConnection.ExecuteScalarAsync<int>(getKeySql);

            var insertSql = @"
                INSERT INTO hr.employees ([key], name, surname, position, branch)
                VALUES (@key, @name, @surname, @position, @branch)";
            Console.WriteLine(insertSql);

            await _dbConnection.ExecuteAsync(insertSql, new
            {
                key = newKey,
                name = employee.Name,
                surname = employee.Surname,
                position = employee.Position,
                branch = employee.Branch
            });

            return new Employee
            {
                Key = newKey,
                Name = employee.Name,
                Surname = employee.Surname,
                Position = employee.Position,
                Branch = employee.Branch
            };
        }

        public async Task<Employee?> UpdateAsync(int key, EmployeeDTO employee)
        {
            var updateSql = @"
                UPDATE hr.employees
                SET name = @name, surname = @surname, position = @position, branch = @branch
                WHERE [key] = @key";

            var affectedRows = await _dbConnection.ExecuteAsync(updateSql, new
            {
                key,
                name = employee.Name,
                surname = employee.Surname,
                position = employee.Position,
                branch = employee.Branch
            });

            return affectedRows > 0 ? await GetByIdAsync(key) : null;
        }

        public async Task<Employee?> PatchAsync(int key, EmployeePatchDTO patch)
        {
            var existing = await GetByIdAsync(key);
            if (existing is null) return null;

            var updateSql = @"
                UPDATE hr.employees
                SET name = @name,
                    surname = @surname,
                    position = @position,
                    branch = @branch
                WHERE [key] = @key";

            await _dbConnection.ExecuteAsync(updateSql, new
            {
                key,
                name = patch.Name ?? existing.Name,
                surname = patch.Surname ?? existing.Surname,
                position = patch.Position ?? existing.Position,
                branch = patch.Branch ?? existing.Branch
            });

            return await GetByIdAsync(key);
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var deleteSql = "DELETE FROM hr.employees WHERE [key] = @key";
            var affectedRows = await _dbConnection.ExecuteAsync(deleteSql, new { key });
            return affectedRows > 0;
        }
    }
}
