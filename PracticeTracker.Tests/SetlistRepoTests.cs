using App.Data.Context;
using App.Models;
using App.Repo;
using Moq;

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
            var setlist = new Setlist
            {
                Id = 1,
                UserId = 1,
                Name = "Gig Setlist",
                CreatedAt = DateTime.Now,
                SetlistSongs = new List<SetlistSong>
                {
                    new SetlistSong
                    {
                        SetlistId = 1,
                        SongId = 101,
                        SongOrder = 1,
                        Notes = "Opener",
                        Song = new Song{Id = 101, Title = "Song A", Key = "C", BPM = 120, DurationMinutes = 3 }
                    },
                    new SetlistSong
                    {
                        SetlistId = 1,
                        SongId = 102,
                        SongOrder = 2,
                        Notes = "Closer",
                        Song = new Song{Id = 102, Title = "Song B", Key = "G", BPM = 100, DurationMinutes = 4 }
                    }

                }
            };

            _mockDbContext.Setup(db => db.LoadData<Setlist, object>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                false
                )).Returns(new List<Setlist> { setlist });

            // Act
            var result = _repo.GetSetlistWithSongs(1);

            // Assert 
            Assert.NotNull(result);
            Assert.Equal(2, result.SetlistSongs.Count);
            Assert.Equal(101, result.SetlistSongs[0].Song.Id);
            Assert.Equal("Song A", result.SetlistSongs[0].Song.Title);
            Assert.Equal(102, result.SetlistSongs[1].Song.Id);
        }

        //        [Fact]
        //        public void Update_ShouldModifySetlist_WhenCalledWithValidSession()
        //        {
        //            // Arrange

        //            var setlist = new Setlist
        //            {
        //                Id = 1,
        //                UserId = 1,
        //                Name = "My Jams",
        //                CreatedAt = DateTime.Now

        //            };

        //            // Act
        //            _repo.Update(setlist);

        //            // Assert
        //#pragma warning disable CS8602 // Dereference of a possibly null reference.
        //            _mockDbContext.Verify(db => db.SaveData<PracticeSession, object>(
        //                       It.Is<string>(sql => sql.Contains("UPDATE PracticeSessions")),
        //                       It.Is<object>(param =>
        //                           param.GetType().GetProperty("Date").GetValue(param).Equals(session.Date) &&
        //                           param.GetType().GetProperty("DurationMinutes").GetValue(param).Equals(session.DurationMinutes) &&
        //                           param.GetType().GetProperty("FocusArea").GetValue(param).Equals(session.FocusArea) &&
        //                           param.GetType().GetProperty("Notes").GetValue(param).Equals(session.Notes) &&
        //                           param.GetType().GetProperty("Id").GetValue(param).Equals(session.Id)
        //                       ),
        //                       It.IsAny<string>(),
        //                       false
        //                   ), Times.Once);
        //#pragma warning restore CS8602 // Dereference of a possibly null reference.
        //        }

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
            if (idProperty == null) return false; // Ensure the property exists

            var value = idProperty.GetValue(param);
            return value is int intValue && intValue == expectedId;
        }

    }
}

