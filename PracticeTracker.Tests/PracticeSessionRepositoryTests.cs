using App.Data.Context;
using App.Models;
using App.Repo;
using Moq;

namespace App.Tests.Repo
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
        public void Create_ShouldSaveSession_WhenCalledWithValidSession()
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
            _mockDbContext.Setup(db => db.LoadData<PracticeSession, object>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), false))
                          .Returns(practiceSessions);

            // Act
            var result = _repo.GetAll();

            // Assert
            Assert.Equal(practiceSessions, result);
        }

        [Fact]
        public void GetById_ShouldThrowNotImplementedException()
        {
            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _repo.GetById(It.IsAny<int>()));
        }


        [Fact]
        public void Update_ShouldThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => _repo.Update(It.IsAny<int>()));
        }

        [Fact]
        public void Delete_ShouldThrowNotImplementedException()
        {
            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _repo.Delete());
        }
    }
}
