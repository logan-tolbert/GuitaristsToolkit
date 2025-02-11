using App.Data.Context;
using App.Models;
using App.Repo;
using Moq;

namespace PracticeTracker.Tests
{
    public class PracticeSessionRepoTests
    {
        private readonly Mock<ISqlDbContext> _mockDbContext;
        private readonly PracticeSessionRepo _repo;

        public PracticeSessionRepoTests()
        {
            _mockDbContext = new Mock<ISqlDbContext>();
            _repo = new PracticeSessionRepo(_mockDbContext.Object);
        }

        [Fact]
        public void Create_ShouldSaveSession()
        {
            // Arrange
            var session = new PracticeSession
            {
                UserId = 1,
                Date = DateTime.Now,
                DurationMinutes = 60,
                FocusArea = "Technique",
                Notes = "Focus on scales"
            };

            // Act 
            _repo.Create(session);

            // Assert
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _mockDbContext.Verify(db => db.SaveData<PracticeSession, object>(
                       It.Is<string>(sql => sql.Contains("INSERT INTO PracticeSessions")),
                       It.Is<object>(param =>
                           param.GetType().GetProperty("UserId").GetValue(param).Equals(session.UserId) &&
                           param.GetType().GetProperty("Date").GetValue(param).Equals(session.Date) &&
                           param.GetType().GetProperty("DurationMinutes").GetValue(param).Equals(session.DurationMinutes) &&
                           param.GetType().GetProperty("FocusArea").GetValue(param).Equals(session.FocusArea) &&
                           param.GetType().GetProperty("Notes").GetValue(param).Equals(session.Notes)
                       ),
                       It.IsAny<string>(),
                       false
                   ), Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        [Fact]
        public void GetAll_ShouldReturnAllPracticeSessions()
        {
            // Arrange
            var practiceSessions = new List<PracticeSession>
                {
                    new PracticeSession { Id = 1, UserId = 1, Date = DateTime.Now, DurationMinutes = 60, FocusArea = "Technique" },
                    new PracticeSession { Id = 2, UserId = 2, Date = DateTime.Now, DurationMinutes = 45, FocusArea = "Repertoire" }
                };

            _mockDbContext.Setup(db => db.LoadData<PracticeSession, object>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                false))
                .Returns(practiceSessions);

            // Act
            var result = _repo.GetAll();

            // Assert
            Assert.Equal(practiceSessions, result);
        }

        [Fact]
        public void GetById_ShouldReturnMatch_WhenValidIdIsProvided()
        {
            // Arrange
            var id = 1;

            var expectedSession = new PracticeSession
            {
                Id = 1,
                UserId = 1,
                Date = DateTime.Now,
                DurationMinutes = 60,
                FocusArea = "Technique"
            };

            var practiceSessions = new List<PracticeSession>
            {
             expectedSession,
            new PracticeSession { Id = 2, UserId = 2, Date = DateTime.Now, DurationMinutes = 45, FocusArea = "Scales" }
            };

            _mockDbContext.Setup(db => db.LoadData<PracticeSession, object>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                false))
                .Returns(practiceSessions.Where(ps => ps.Id == id));

            // Act
            var result = _repo.GetById(id);

            // Assert
            Assert.Equal(expectedSession, result);
        }

        [Fact]
        public void Update_ShouldModifySession_WhenCalledWithValidSession()
        {
            // Arrange
            var session = new PracticeSession
            {
                Id = 1,
                UserId = 1,
                Date = DateTime.Now,
                DurationMinutes = 60,
                FocusArea = "Technique",
                Notes = "Focus on arpeggios"
            };

            // Act
            _repo.Update(session);

            // Assert
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _mockDbContext.Verify(db => db.SaveData<PracticeSession, object>(
                       It.Is<string>(sql => sql.Contains("UPDATE PracticeSessions")),
                       It.Is<object>(param =>
                           param.GetType().GetProperty("Date").GetValue(param).Equals(session.Date) &&
                           param.GetType().GetProperty("DurationMinutes").GetValue(param).Equals(session.DurationMinutes) &&
                           param.GetType().GetProperty("FocusArea").GetValue(param).Equals(session.FocusArea) &&
                           param.GetType().GetProperty("Notes").GetValue(param).Equals(session.Notes) &&
                           param.GetType().GetProperty("Id").GetValue(param).Equals(session.Id)
                       ),
                       It.IsAny<string>(),
                       false
                   ), Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }


        [Fact]
        public void Delete_ShouldDeleteSession()
        {
            // Arrange
            var id = 1;

            // Act
            _repo.Delete(id);

            // Assert
            _mockDbContext.Verify(db => db.SaveData<PracticeSession, object>(
                It.Is<string>(sql => sql.Contains("DELETE FROM PracticeSessions WHERE Id = @Id")),
                It.Is<object>(param => IsMatchingId(param, id)),
                It.IsAny<string>(),
                false
            ), Times.Once); 
        }
        private bool IsMatchingId(object param, int expectedId)
        {
            var idProperty = param.GetType().GetProperty("Id");
            if (idProperty == null) return false;

            var value = idProperty.GetValue(param);
            return value is int intValue && intValue == expectedId;
        }

    }
}
