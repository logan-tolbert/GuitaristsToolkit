namespace PracticeTracker.Tests;

using App.Data.Context;
using App.Models;
using App.Repo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class UserRepoIntegrationTests : IDisposable
{

    private readonly ISqlDbContext _db;


    public UserRepoIntegrationTests()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _db = new SqlDbContext(config);

        CleanupTestData();

    }

    private void InsertTestUser()
    {
        var testUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "TestUser",
            FirstName = "John",
            LastName = "Doe",
            Email = "testuser@example.com",
            PasswordHash = "hashedpassword",
        };

        string sql = @"INSERT INTO Users
                       (Id, Username, FirstName, LastName, Email, PasswordHash)
                        VALUES (@Id, @Username, @FirstName, @LastName, @Email, @PasswordHash);";

        _db.SaveData<User, dynamic>(sql, testUser, "Testing");
        
    }


    private void CleanupTestData()
    {
        string sql = "DELETE FROM Users WHERE Username = @Username";
        var parameters = new { Username = "TestUser" };
        string connectionName = "Testing";

        _db.SaveData<User, dynamic>(sql, parameters, connectionName, false);

    }

    [Fact]
    public void CreateUser_ShouldInsertUserSuccessfully()
    {
        InsertTestUser();
        var sql = @"SELECT * FROM Users WHERE Email = @input OR Username = @input;";
        var usr = "TestUser";
        string connectionName = "Testing";

        var result = _db.LoadData<User, dynamic>(sql, new { input = usr }, connectionName, false).FirstOrDefault();


        Assert.NotNull(result);
        Assert.Equal("TestUser", result.Username);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal("testuser@example.com", result.Email);
    }

    public void Dispose()
    {
        CleanupTestData();
    }
}
