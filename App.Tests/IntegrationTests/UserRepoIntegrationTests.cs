﻿namespace GuitaristsToolkit.Tests.IntegrationTests;

using App.Data.Context;
using App.Models;
using App.Repo;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

public class UserRepoIntegrationTests : IDisposable
{

    private readonly ISqlDbContext _db;
    private readonly IUserRepo _repo;

    public UserRepoIntegrationTests()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _db = new SqlDbContext(config);
        _repo = new UserRepo(_db);

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
        
        CleanupTestData();

        // Arrange
        var testUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "TestUser",
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            PasswordHash = "TestPassword"
        };

        // Act
        var result = _repo.CreateUser(testUser, "Testing");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testUser.Username, result.Username);
        Assert.Equal(testUser.Email, result.Email);
        Assert.True(result.CreatedAt > DateTime.MinValue);
    }



    [Fact]
    public void GetUserByEmailOrUsername_ShouldReturnCorrectUser()
    {
        // Arrange & Act
        InsertTestUser();

        // Assert
        var result = _repo.GetUserByEmailOrUsername("testuser@example.com", "Testing");
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
