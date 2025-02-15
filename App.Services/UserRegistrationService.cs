
using App.Models;
using App.Repo;

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services;

public class UserRegistrationService
{
    private readonly IUserRepo _repo;
    private readonly IPasswordHasher<User> _hasher;

    public UserRegistrationService(IUserRepo repo, IPasswordHasher<User> hasher)
    {
        _repo = repo;
        _hasher = hasher;
    } 

    public bool RegisterUser(string username, string firstName, string lastName, string email, string password, string connectionName)
    {
        if (_repo.GetUserByEmailOrUsername(email, connectionName) != null)
            return false;
        var newUser = new User();
        var hashedPassword = _hasher.HashPassword(newUser, password);



        newUser.Id = Guid.NewGuid();
        newUser.Username = username;
        newUser.FirstName = firstName;
        newUser.LastName = lastName;
        newUser.Email = email;
        newUser.PasswordHash = hashedPassword;

        _repo.Create(newUser, connectionName);

        return true;
        
    }
}
