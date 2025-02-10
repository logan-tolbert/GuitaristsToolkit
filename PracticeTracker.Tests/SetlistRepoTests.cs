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

        }
    }

}
