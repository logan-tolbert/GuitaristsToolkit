using App.Data.Context;
using App.Models;
using App.Repo;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var id = 1;
            var setlist = new Setlist { Id = 1, UserId = 1, Name = "My Jams", CreatedAt = DateTime.Now };

            _mockDbContext.Setup(db => db.LoadData<Setlist, object>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                false
            )).Returns(new List<Setlist> { setlist });

            _repo.Create(setlist);
            Assert.NotNull(_repo.GetById(id));

            _repo.Delete(id);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _mockDbContext.Verify(db => db.SaveData<Setlist, object>(
                It.Is<string>(sql => sql.Contains("DELETE FROM Setlists WHERE Id = @Id")),
                It.Is<object>(param => param.GetType().GetProperty("Id").GetValue(param).Equals(id)),
                It.IsAny<string>(),
                false
            ), Times.Once);


            _mockDbContext.Setup(db => db.LoadData<Setlist, object>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                false
            )).Returns(new List<Setlist>());

            Assert.Null(_repo.GetById(id));
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.



    }
}

