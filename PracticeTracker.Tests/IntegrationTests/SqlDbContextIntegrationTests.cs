namespace GuitaristsToolkit.Tests.IntegrationTests;
using App.Data.Context;
using App.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class SqlDbContextIntegrationTests : IDisposable
{
    private readonly SqlDbContext _db;

    public SqlDbContextIntegrationTests()
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
        string sqlStatement = "INSERT INTO Users (Id, Username, FirstName, LastName, Email, PasswordHash, CreatedAt) VALUES (@Id, @Username, @FirstName, @LastName, @Email, @PasswordHash, @CreatedAt)";
        var parameters = new
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            FirstName = "Test",
            LastName = "User",
            Email = "testuser@example.com",
            PasswordHash = "hashedpassword",
            CreatedAt = DateTime.UtcNow
        };
        string connectionName = "Testing";

        _db.SaveData<dynamic, dynamic>(sqlStatement, parameters, connectionName, false);
    }

    private void CleanupTestData()
    {
        string sql = "DELETE FROM Users WHERE Email = @Email";
        var parameters = new { Email = "testuser@example.com" };
        string connectionName = "Testing";

        _db.SaveData<dynamic, dynamic>(sql, parameters, connectionName, false);
    }

    [Fact]
    public void SaveData_ShouldSaveUserCorrectly()
    {
        // Arrange
        InsertTestUser();

        string verifySql = "SELECT * FROM Users WHERE Email = @Email";
        var verifyParameters = new { Email = "testuser@example.com" };
        string connectionName = "Testing";
        IEnumerable<User> result = _db.LoadData<User, dynamic>(verifySql, verifyParameters, connectionName);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        var user = result.FirstOrDefault();
        Assert.NotNull(user);
        Assert.Equal("testuser", user.Username);
        Assert.Equal("Test", user.FirstName);
        Assert.Equal("User", user.LastName);
        Assert.Equal("testuser@example.com", user.Email);
        Assert.Equal("hashedpassword", user.PasswordHash);
    }

    [Fact]
    public void LoadData_ShouldReturnExpectedUser()
    {
        // Arrange
        InsertTestUser();
        string sqlStatement = "SELECT * FROM Users WHERE Email = @Email";
        var parameters = new { Email = "testuser@example.com" };
        string connectionName = "Testing";

        // Act
        IEnumerable<User> result = _db.LoadData<User, dynamic>(sqlStatement, parameters, connectionName);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        var user = result.FirstOrDefault();
        Assert.NotNull(user);
        Assert.Equal("testuser", user.Username);
        Assert.Equal("Test", user.FirstName);
        Assert.Equal("User", user.LastName);
        Assert.Equal("testuser@example.com", user.Email);
        Assert.Equal("hashedpassword", user.PasswordHash);
    }

    public void Dispose()
    {
        CleanupTestData();
    }
}