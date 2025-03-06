namespace GuitaristsToolkit.Tests.IntegrationTests;
using App.Data.Context;
using App.Models;
using App.Repo;
using App.Security;
using App.Services;
using Microsoft.Extensions.Configuration;
using System;


public class UserAuthenticationServiceTests : IDisposable
{
    private readonly UserAuthenticationService _authService;
    private readonly IUserRepo _repo;
    private readonly ISqlDbContext _db;

    public UserAuthenticationServiceTests()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _db = new SqlDbContext(config);
        _repo = new UserRepo(_db);
        var hasher = new BCryptPasswordHasher();
        _authService = new UserAuthenticationService(_repo, hasher);

        CleanupTestData();
    }

    private void InsertTestUser()
    {
        string sqlStatement = "INSERT INTO Users (Id, Username, FirstName, LastName, Email, PasswordHash) VALUES (@Id, @Username, @FirstName, @LastName, @Email, @PasswordHash)";
        var parameters = new
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            FirstName = "Test",
            LastName = "User",
            Email = "testuser@example.com",
            PasswordHash = new BCryptPasswordHasher().HashPassword(null, "password123")
        };
        string connectionName = "Testing";

        _db.SaveData<User, dynamic>(sqlStatement, parameters, connectionName, false);
    }

    private void CleanupTestData()
    {
        string sql = "DELETE FROM Users WHERE Email = @Email";
        var parameters = new { Email = "testuser@example.com" };
        string connectionName = "Testing";

        _db.SaveData<User, dynamic>(sql, parameters, connectionName, false);
    }

    [Fact]
    public void AuthenticateUser_ShouldReturnTrue_WhenCredentialsAreValid()
    {
        // Arrange
        InsertTestUser();

        // Act
        var result = _authService.AuthenticateUser("testuser@example.com", "password123", out var authenticatedUser, "Testing");

        // Assert
        Assert.True(result);
        Assert.NotNull(authenticatedUser);
        Assert.Equal("testuser", authenticatedUser.Username);
        Assert.Equal("Test", authenticatedUser.FirstName);
        Assert.Equal("User", authenticatedUser.LastName);
        Assert.Equal("testuser@example.com", authenticatedUser.Email);
    }

    [Fact]
    public void AuthenticateUser_ShouldReturnFalse_WhenPasswordIsIncorrect()
    {
        // Arrange
        InsertTestUser();

        // Act
        var result = _authService.AuthenticateUser("testuser@example.com", "wrongpassword", out var authenticatedUser, "Testing");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AuthenticateUser_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Act
        var result = _authService.AuthenticateUser("nonexistent@example.com", "password123", out var authenticatedUser, "Testing");

        // Assert
        Assert.False(result);
    }
    public void Dispose()
    {
        CleanupTestData();
    }
}
