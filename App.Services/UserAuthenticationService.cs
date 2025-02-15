namespace App.Services;
using App.Models;
using App.Repo;
using Microsoft.AspNetCore.Identity;



public class UserAuthenticationService
{
    private readonly IUserRepo _repo;
    private readonly IPasswordHasher<User> _hasher;

    public UserAuthenticationService(IUserRepo repo, IPasswordHasher<User> hasher)
    {
        _repo = repo;
        _hasher = hasher;
    }

    public bool AuthenticateUser(string emailOrUsername, string password, out User? authenticatedUser, string connectionName = "Default")
    {
        authenticatedUser = _repo.GetUserByEmailOrUsername(emailOrUsername, connectionName);
        if (authenticatedUser == null || string.IsNullOrEmpty(authenticatedUser.PasswordHash))
        {
            authenticatedUser = null;
            return false;
        }

        var result = _hasher.VerifyHashedPassword(null, authenticatedUser.PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }

}
