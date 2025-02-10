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
        public void Create_ShouldCreateNewSetlistAndReturnId()
        {
            // Arrange 
            var setList = new Setlist
            {
                UserId = 1,
                Name = "Practice Set",
                CreatedAt = DateTime.Now
            };

            _mockDBContext.Setup(db => db.LoadData<int, dynamic>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                false
               )).Returns(new List<int> { 1 });

            // Act
            var result = _repo.Create(setList);

            // Assert

            Assert.Equal(1, result);

            _mockDBContext.Verify(db => db.SaveData<Setlist, object>(
                It.Is<string>(sql => sql.Contains("INSERT INTO SetLists")),
                It.Is<object>(param => param.GetType().GetProperty("UserId").GetValue(param).Equals(setList.UserId) &&
                param.GetType().GetProperty("Name").GetValue(param).Equals(setList.Name) &&
                param.GetType().GetProperty("CreatedAt").GetValue(param).Equals(setList.CreatedAt)
                ),
                It.IsAny<string>(),
                false
                ), Times.Once);

        }
    }

}
