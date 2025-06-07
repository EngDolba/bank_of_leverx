using BankOfLeverx.Domain.Models;
using Dapper;
using System.Data;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var sql = @"SELECT [Key], username, hashedPassword, Role
                        FROM krn.Users";
            return await _dbConnection.QueryAsync<User>(sql);
        }

        public async Task<User?> GetByIdAsync(int key)
        {
            var sql = @"SELECT [Key], username, hashedPassword, Role
                        FROM krn.Users
                        WHERE [Key] = @key";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { key });
        }

        public async Task<User> CreateAsync(User user)
        {
            var getKeySql = "SELECT NEXT VALUE FOR krn.User_seq";
            var newKey = await _dbConnection.ExecuteScalarAsync<int>(getKeySql);

            var insertSql = @"
                INSERT INTO krn.Users ([Key], username, hashedPassword, Role)
                VALUES (@Key, @username, @hashedPassword, @Role)";

            await _dbConnection.ExecuteAsync(insertSql, new
            {
                Key = newKey,
                username = user.Username,
                hashedPassword = user.HashedPassword,
                Role = user.Role
            });
            user. Key = newKey;
            return user;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var updateSql = @"
                UPDATE krn.Users SET
                    username = @username,
                    hashedPassword = @hashedPassword,
                    Role = @Role
                WHERE [Key] = @Key";

            var affectedRows = await _dbConnection.ExecuteAsync(updateSql, new
            {
                Key = user.Key,
                username = user.Username,
                hashedPassword = user.HashedPassword,
                Role = user.Role
            });

            return affectedRows > 0 ? await GetByIdAsync(user.Key) : null;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var deleteSql = "DELETE FROM krn.Users WHERE [Key] = @Key";
            var affectedRows = await _dbConnection.ExecuteAsync(deleteSql, new { Key = key });
            return affectedRows > 0;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var getSql = "SELECT [Key], username, hashedPassword, Role FROM krn.Users WHERE username = @Username";
            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(getSql, new
            {
                Username = username
            });
            Console.WriteLine(user.Role+"rep");
            return user;
        }
    }
}
