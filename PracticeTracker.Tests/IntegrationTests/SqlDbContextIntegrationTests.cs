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

    private void InsertTestData()
    {
        string sqlStatement = "INSERT INTO PracticeSessions (UserId, CreatedAt, DurationMinutes, FocusArea, Notes) VALUES (@UserId, @CreatedAt, @DurationMinutes, @FocusArea, @Notes)";
        var parameters = new
        {
            UserId = 1,
            CreatedAt = new DateTime(2023, 10, 1),
            DurationMinutes = 30,
            FocusArea = "New Focus Area",
            Notes = "Some notes"
        };
        string connectionName = "Testing";

        _db.SaveData<dynamic, dynamic>(sqlStatement, parameters, connectionName, false);
    }

    private void CleanupTestData()
    {
        string sql = "DELETE FROM PracticeSessions WHERE UserId = @UserId";
        var parameters = new { UserId = 1 };
        string connectionName = "Testing";

        _db.SaveData<dynamic, dynamic>(sql, parameters, connectionName, false);
    }

    [Fact]
    public void SaveData_ShouldSaveDataCorrectly()
    {
        // Arrange
        InsertTestData();

        string verifySql = "SELECT * FROM PracticeSessions WHERE UserId = @UserId AND FocusArea = @FocusArea";
        var verifyParameters = new { UserId = 1, FocusArea = "New Focus Area" };
        string connectionName = "Testing";
        IEnumerable<PracticeSession> result = _db.LoadData<PracticeSession, dynamic>(verifySql, verifyParameters, connectionName);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        var session = result.FirstOrDefault();
        Assert.NotNull(session);
        Assert.Equal("New Focus Area", session.FocusArea);
    }

    [Fact]
    public void LoadData_ShouldReturnExpectedResults()
    {
        // Arrange
        InsertTestData();
        string sqlStatement = "SELECT * FROM PracticeSessions WHERE UserId = @UserId";
        var parameters = new { UserId = 1 };
        string connectionName = "Testing";

        // Act
        IEnumerable<PracticeSession> result = _db.LoadData<PracticeSession, dynamic>(sqlStatement, parameters, connectionName);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        var session = result.FirstOrDefault();
        Assert.NotNull(session);
        Assert.Equal(1, session.UserId);
        Assert.Equal(new DateTime(2023, 10, 1), session.CreatedAt);
        Assert.Equal(30, session.DurationMinutes);
        Assert.Equal("New Focus Area", session.FocusArea);
        Assert.Equal("Some notes", session.Notes);
    }

    public void Dispose()
    {
        CleanupTestData();
    }
}



