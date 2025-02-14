using App.Models;

namespace App.Repo
{
    public interface IUserRepo
    {
        void CreateUser(User user);
        User? GetUserByEmailOrUsername(string input, string connectionName = "Default");
    }
}