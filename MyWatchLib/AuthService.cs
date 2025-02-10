using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MyWatchLib
{
    public class AuthService
    {
        private readonly DbHelper _dbHelper;

        public AuthService(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // Hasher password med SHA256 + salt
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var salt = "some_random_salt"; // Brug en salt værdi, der er unik for hver bruger
                var saltedPassword = password + salt;
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public async Task<bool> RegisterUser(string username, string password, string role)
        {
            // Check om brugernavnet allerede findes
            var checkQuery = "SELECT COUNT(*) FROM users WHERE Username = @Username";
            var parameters = new[]
            {
            new SqlParameter("@Username", username)
        };

            var result = await _dbHelper.ExecuteQueryAsync(checkQuery, parameters);
            if (result.Rows[0][0].ToString() != "0")
            {
                throw new Exception("Brugernavnet er allerede taget");
            }

            // Hash password og opret ny bruger
            var hashedPassword = HashPassword(password);
            var insertQuery = "INSERT INTO users (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, @Role)";
            var insertParams = new[]
            {
            new SqlParameter("@Username", username),
            new SqlParameter("@PasswordHash", hashedPassword),
            new SqlParameter("@Role", role)
        };

            await _dbHelper.ExecuteNonQueryAsync(insertQuery, insertParams);
            return true;
        }

        public async Task<User> Login(string username, string password)
        {
            var query = "SELECT Id, Username, PasswordHash, Role FROM users WHERE Username = @Username";
            var parameters = new[]
            {
            new SqlParameter("@Username", username)
        };

            var result = await _dbHelper.ExecuteQueryAsync(query, parameters);

            if (result.Rows.Count == 0)
                throw new Exception("Bruger ikke fundet");

            var user = result.Rows[0];
            var hashedPassword = user["PasswordHash"].ToString();

            if (hashedPassword != HashPassword(password))
                throw new Exception("Forkert kodeord");

            return new User
            {
                Id = Convert.ToInt32(user["Id"]),
                Username = user["Username"].ToString(),
                Role = user["Role"].ToString()
            };
        }
    }
}
