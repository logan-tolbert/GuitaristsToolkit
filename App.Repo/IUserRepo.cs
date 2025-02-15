using App.Models;

namespace App.Repo
{
    public interface IUserRepo
    {
        void Create(User user, string connectionName = "Default");
        User? GetUserByEmailOrUsername(string input, string connectionName = "Default");
    }
}