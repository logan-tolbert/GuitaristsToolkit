using App.Models;

namespace App.Repo
{
    public interface IUserRepo
    {
        public User? CreateUser(User user, string connectionName = "Default");
        User? GetUserByEmailOrUsername(string input, string connectionName = "Default");
    }
}