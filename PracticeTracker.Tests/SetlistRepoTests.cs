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
        private readonly Mock<ISqlDbContext> _mockDBContext;
        private readonly SetlistRepo _repo;

        public SetlistRepoTests()
        {
            _mockDBContext = new Mock<ISqlDbContext>();
            _repo = new SetlistRepo(_mockDBContext.Object);
        }

        [Fact]
        public void Create_ShouldCreateNewSetlist()
        {
            // Arrange 
            var setList = new Setlist
            {
                UserId = 1,
                Name = "Practice Set",
                CreatedAt = DateTime.Now
            };

            // Act
            _repo.Create(setList);

            // Assert
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _mockDBContext.Verify(db => db.SaveData<Setlist, object>(
                It.Is<string>(sql => sql.Contains("INSERT INTO SetLists")),
                It.Is<object>(param => param.GetType().GetProperty("UserId").GetValue(param).Equals(setList.UserId) &&
                param.GetType().GetProperty("Name").GetValue(param).Equals(setList.Name) &&
                param.GetType().GetProperty("CreatedAt").GetValue(param).Equals(setList.CreatedAt)
                ),
                It.IsAny<string>(),
                false
                ), Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }

}
