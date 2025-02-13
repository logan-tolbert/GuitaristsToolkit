using App.Data.Context;
using App.Models;
using App.Repo;
using Moq;
using PracticeTracker.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace PracticeTracker.Tests
{
    public class SongRepoTests
    {
        private readonly Mock<ISqlDbContext> _mockDbContext;
        private readonly SongRepo _repo;


        public SongRepoTests()
        {
            _mockDbContext = new Mock<ISqlDbContext>();
            _repo = new SongRepo(_mockDbContext.Object);
        }

        [Fact]
        public void Create_ShouldCreateNewSongAndReturnID()
        {
            // Arrange 
            var song = new Song
            {
                Id = 101,
                Title = "Crazy Train",
                Key = "F-sharp minor",
                BPM = 135,
                DurationMinutes = 5,
                Notes = "Live temp: 137"
            };

            var expected = 101;

            _mockDbContext.Setup(db => db.LoadData<int, dynamic>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                false
                )).Returns(new List<int> { expected });

            // Act 
            var result = _repo.Create(song);

            // Assert 
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetALL_ShouldReturnAllSongs()
        {
            // Arrange 
            var songs = new List<Song>
            {
                new Song
                {
                    Id = 101,
                    Title = "Crazy Train",
                    Key = "F-sharp minor",
                    BPM = 135,
                    DurationMinutes = 5,
                    Notes = "Live temp: 137"
                },
                new Song
                {
                    Id = 102,
                    Title = "Sweet Child O' Mine",
                    Key = "D major",
                    BPM = 125,
                    DurationMinutes = 6,
                    Notes = "Intro picking pattern"
                }
            };

            _mockDbContext.Setup(db => db.LoadData<Song, object>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<string>(),
                false
                )).Returns(songs);

            // Act 
            var result = _repo.GetAll();


            // Assert
            Assert.Equal(songs, result);
        }

        [Fact]
        public void Update_ShouldModifySong_WhenCalledWithValidSong()
        {
            // Arrange
            var song = new Song
            {
                Id = 106,
                Title = "Purple Haze",
                Key = "E minor",
                BPM = 114,
                DurationMinutes = 3,
                Notes = "Watch for triplets in solo"
            };

            // Act
            _repo.Update(song);

            // Assert 
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _mockDbContext.Verify(db => db.SaveData<Song, object>(
                It.Is<string>(sql => sql.Contains("UPDATE Songs")),
                It.Is<object>(param => param.GetType().GetProperty("Title").GetValue(param).Equals(song.Title) &&
                param.GetType().GetProperty("Key").GetValue(param).Equals(song.Key) &&
                param.GetType().GetProperty("BPM").GetValue(param).Equals(song.BPM) &&
                param.GetType().GetProperty("DurationMinutes").GetValue(param).Equals(song.DurationMinutes) &&
                param.GetType().GetProperty("Notes").GetValue(param).Equals(song.Notes)), It.IsAny<string>(),
                false
                ), Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

      


        [Fact]
        public void Delete_ShouldDeleteSong()
        {
            // Arrange
            var id = 1;

            // Act
            _repo.Delete(id);

            // Assert
            _mockDbContext.Verify(db => db.SaveData<Song, object>(
                It.Is<string>(sql => sql.Contains("DELETE FROM Songs WHERE Id = @Id")),
                It.Is<object>(param => TestHelper.IsMatchingId(param, id)),
                It.IsAny<string>(),
                false
            ), Times.Once);
        }

    }
}
