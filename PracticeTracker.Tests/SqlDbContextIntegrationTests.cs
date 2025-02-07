using App.Data.Context;
using App.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;

namespace PracticeTracker.Tests
{
    public class SqlDbContextIntegrationTests
    {
        private readonly SqlDbContext _sqlDbContext;

        public SqlDbContextIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _sqlDbContext = new SqlDbContext(configuration);
        }

        [Fact]
        public void LoadData_ShouldReturnExpectedResults()
        {
            // Arrange
            string sqlStatement = "SELECT * FROM PracticeSessions WHERE UserId = @UserId";
            var parameters = new { UserId = 1 };
            string connectionName = "Default";

            // Act
            IEnumerable<PracticeSession> result = _sqlDbContext.LoadData<PracticeSession, dynamic>(sqlStatement, parameters, connectionName);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result.First().UserId);
        }

        [Fact]
        public void SaveData_ShouldSaveDataCorrectly()
        {
            // Arrange
            string sqlStatement = "INSERT INTO PracticeSessions (UserId, Date, DurationMinutes, FocusArea, Notes) VALUES (@UserId, @Date, @DurationMinutes, @FocusArea, @Notes)";
            var parameters = new
            {
                UserId = 1,
                Date = new DateTime(2023, 10, 1),
                DurationMinutes = 30,
                FocusArea = "New Focus Area",
                Notes = "Some notes"
            };
            string connectionName = "Default";

            // Act
            _sqlDbContext.SaveData<dynamic, dynamic>(sqlStatement, parameters, connectionName, false);

            // Verify by loading the data
            string verifySql = "SELECT * FROM PracticeSessions WHERE UserId = @UserId AND FocusArea = @FocusArea";
            var verifyParameters = new { UserId = 1, FocusArea = "New Focus Area" };
            IEnumerable<PracticeSession> result = _sqlDbContext.LoadData<PracticeSession, dynamic>(verifySql, verifyParameters, connectionName);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal("New Focus Area", result.First().FocusArea);
        }
    }
}
