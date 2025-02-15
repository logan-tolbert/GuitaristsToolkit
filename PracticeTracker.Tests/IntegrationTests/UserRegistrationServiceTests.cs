namespace GuitaristsToolkit.Tests.IntegrationTests;
using App.Data.Context;
using App.Models;
using App.Repo;
using App.Security;
using App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


public class UserRegistrationServiceTests : IDisposable
{
    private readonly UserRegistrationService _registration;
    private readonly IUserRepo _repo;
    private readonly ISqlDbContext _db;

    public UserRegistrationServiceTests()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _db = new SqlDbContext(config);
        _repo = new UserRepo(_db);
        var hasher = new BCryptPasswordHasher();
        _registration = new UserRegistrationService(_repo, hasher);

        CleanupTestData();
    }

    private void CleanupTestData()
    {
        string sql = "DELETE FROM Users WHERE Email LIKE '%@example.com'";
        string connectionName = "Testing";

        _db.SaveData<User, dynamic>(sql, new { }, connectionName, false);
    }

    [Fact]
    public void RegisterUser_ShouldRegisterSuccessfully_WhenUserDoesNotExist()
    {
        // Arrange 
        var username = "TestUser";
        var firstName = "John";
        var lastName = "doe";
        var email = "testuser@example.com";
        var password = "password";
        string connectionName = "Testing";

        // Act
        var result = _registration.RegisterUser(username, firstName, lastName, email, password, connectionName);
        var newUser = _repo.GetUserByEmailOrUsername(username, connectionName);

        // Assert
        Assert.True(result);
        Assert.NotNull(newUser);
        Assert.Equal(username, newUser.Username);
        Assert.Equal(email, newUser.Email);
        Assert.NotEqual(password, newUser.PasswordHash); 
    }

    [Fact]
    public void RegisterUser_ShouldFail_WhenUserAlreadyExists()
    {
        
        CleanupTestData();

        // Arrange 
        var result1 = _registration.RegisterUser("TestUser", "John", "Doe", "test@example.com", "TestPassword", "Testing");
        Assert.True(result1); 

        // Act 
        var result2 = _registration.RegisterUser("TestUser", "John", "Doe", "test@example.com", "TestPassword", "Testing");

        // Assert 
        Assert.False(result2);
    }



    [Fact]
    public void RegisterUser_ShouldHashPasswordBeforeStoring()
    {
        // Arrange 
        var username = "TestUser";
        var firstName = "John";
        var lastName = "doe";
        var email = "testuser@example.com";
        var password = "password";
        string connectionName = "Testing";

        // Act
        _registration.RegisterUser(username, firstName, lastName, email, password, connectionName);
        var user = _repo.GetUserByEmailOrUsername(email, connectionName);

        // Assert
        Assert.NotNull(user);
        Assert.NotEqual(password, user.PasswordHash);
        Assert.True(new BCryptPasswordHasher().VerifyHashedPassword(null, user.PasswordHash, password) == PasswordVerificationResult.Success);
    }

    public void Dispose()
    {
        CleanupTestData();
    }
}
