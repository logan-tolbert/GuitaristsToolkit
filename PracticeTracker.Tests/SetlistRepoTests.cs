using App.Data.Context;
using App.Models;
using App.Repo;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using Xunit.Abstractions;
using static App.Repo.SetlistRepo;

namespace PracticeTracker.Tests
{

    public class SetlistRepoTests
    {
        private readonly Mock<ISqlDbContext> _mockDbContext;
        private readonly SetlistRepo _repo;


        public SetlistRepoTests()
        {
            _mockDbContext = new Mock<ISqlDbContext>();
            _repo = new SetlistRepo(_mockDbContext.Object);
        }
        [Fact]
        public void Create_ShouldCreateNewSetlistAndReturnId()
        {
            // Arrange 
            var setList = new Setlist
            {
                UserId = 1,
                Name = "Practice Set",
                CreatedAt = DateTime.Now
            };

            _mockDbContext.Setup(db => db.LoadData<int, dynamic>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                false
               )).Returns(new List<int> { 1 });

            // Act
            var result = _repo.Create(setList);

            // Assert
            Assert.Equal(1, result);

        }

        [Fact]
        public void GetAll_ShouldReturnAllSetlists()
        {
            // Arrange
            var setlists = new List<Setlist>
                {
                    new Setlist { Id = 1, UserId = 1, Name = "My Jams", CreatedAt = DateTime.Now},
                    new Setlist { Id = 2, UserId = 2, Name = "Favorites", CreatedAt = DateTime.Now}
                };

            _mockDbContext.Setup(db => db.LoadData<Setlist, object>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                false))
                .Returns(setlists);

            // Act
            var result = _repo.GetAll();

            // Assert
            Assert.Equal(setlists, result);
        }

        [Fact]
        public void GetById_ShouldReturnMatch_WhenValidIdIsProvided()
        {
            // Arrange
            var id = 1;

            var expected = new Setlist
            {
                Id = 1,
                UserId = 1,
                Name = "My Jams",
                CreatedAt = DateTime.Now

            };

            var setlists = new List<Setlist>
            {
                 expected,
                 new Setlist { Id = 2, UserId = 2, Name = "Favorites", CreatedAt = DateTime.Now}
            };

            _mockDbContext.Setup(db => db.LoadData<Setlist, object>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                false))
                .Returns(setlists.Where(s => s.Id == id));

            // Act
            var result = _repo.GetById(id);

            // Assert
            Assert.Equal(expected, result);
        }
        [Fact]
        public void GetSetlistWithSongs_ShouldReturnSetlistWithSongs_WhenValidIdIsProvided()
        {
            // Arrange
            var id = 1;
            var setlistData = new List<SetlistSongResult>
        {
            new SetlistSongResult
            {
                Id = 1, UserId = 1, Name = "Setlist 1", CreatedAt = DateTime.Now,
                SongId = 1, SongOrder = 1, Notes = "Note 1", Title = "Song 1", Key = "C", BPM = 120, DurationMinutes = 3
            },
            new SetlistSongResult
            {
                Id = 1, UserId = 1, Name = "Setlist 1", CreatedAt = DateTime.Now,
                SongId = 2, SongOrder = 2, Notes = "Note 2", Title = "Song 2", Key = "D", BPM = 130, DurationMinutes = 4
            }
        };

            _mockDbContext.Setup(db => db.LoadData<SetlistSongResult, dynamic>(
                It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(setlistData);

            // Act
            var result = _repo.GetSetlistWithSongs(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.SetlistSongs.Count);
            Assert.Equal("Song 1", result.SetlistSongs[0].Song.Title);
            Assert.Equal("Song 2", result.SetlistSongs[1].Song.Title);
        }

        //TODO: Implement update test

        [Fact]
        public void Delete_ShouldRemoveSetlist()
        {
            // Arrange
            var id = 1;

            // Act
            _repo.Delete(id);

            // Assert
            _mockDbContext.Verify(db => db.SaveData<Setlist, object>(
                It.Is<string>(sql => sql.Contains("DELETE FROM Setlists WHERE Id = @Id")),
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

