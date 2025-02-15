using App.Data.Context;
using App.Models;

namespace App.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly ISqlDbContext _db;

        public UserRepo(ISqlDbContext db)
        {
            _db = db;
        }

        public User? GetUserByEmailOrUsername(string input, string connectionName = "Default")
        {
            var sql = @"SELECT TOP 1 * FROM Users WHERE Email = @input OR Username = @input;";
            return _db.LoadData<User, dynamic>(sql, new { input }, connectionName).FirstOrDefault();
        }

        public User? CreateUser(User user, string connectionName = "Default")
        {
            var sql = @"INSERT INTO Users (Id, Username, FirstName, LastName, Email, PasswordHash)
                      VALUES (@Id, @Username, @FirstName, @LastName, @Email, @PasswordHash);";

            _db.SaveData<User, dynamic>(sql, new
            {
                user.Id,
                user.Username,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PasswordHash
            }, connectionName);

            sql = "SELECT * FROM Users WHERE Id = @Id;";

            return _db.LoadData<User, dynamic>(sql, new { user.Id }, connectionName).FirstOrDefault();
        }

    }
}
