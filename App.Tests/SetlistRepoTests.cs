﻿namespace GuitaristsToolkit.Tests;
using App.Data.Context;
using App.Models;
using App.Repo;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

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
        var userId = Guid.NewGuid();
        var setList = new Setlist
        {
            UserId = userId,
            Name = "Practice Set",
            CreatedAt = DateTime.Now
        };

        _mockDbContext.Setup(db => db.LoadData<int, dynamic>(
            It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), false))
            .Returns(new List<int> { 1 });

        var result = _repo.Create(setList);

        Assert.Equal(1, result);
    }

    [Fact]
    public void GetAll_ShouldReturnAllSetlists()
    {
        var setlists = new List<Setlist>
        {
            new Setlist { Id = 1, UserId = Guid.NewGuid(), Name = "My Jams", CreatedAt = DateTime.Now},
            new Setlist { Id = 2, UserId = Guid.NewGuid(), Name = "Favorites", CreatedAt = DateTime.Now}
        };

        _mockDbContext.Setup(db => db.LoadData<Setlist, object>(
            It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), false))
            .Returns(setlists);

        var result = _repo.GetAll();

        Assert.Equal(setlists, result);
    }

    [Fact]
    public void GetById_ShouldReturnMatch_WhenValidIdIsProvided()
    {
        var id = 1;
        var expected = new Setlist
        {
            Id = 1,
            UserId = Guid.NewGuid(),
            Name = "My Jams",
            CreatedAt = DateTime.Now
        };

        var setlists = new List<Setlist> { expected };

        _mockDbContext.Setup(db => db.LoadData<Setlist, object>(
            It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), false))
            .Returns(setlists.Where(s => s.Id == id));

        var result = _repo.GetById(id);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetSetlistWithSongs_ShouldReturnSetlistWithSongs_WhenValidIdIsProvided()
    {
        var id = 1;
        var userId = Guid.NewGuid();
        var setlistData = new List<SetlistSongResult>
        {
            new SetlistSongResult
            {
                Id = 1, UserId = userId, Name = "Setlist 1", CreatedAt = DateTime.Now,
                SongId = 1, SongOrder = 1, Notes = "Note 1", Title = "Song 1", Key = "C", BPM = 120, DurationMinutes = 3
            },
            new SetlistSongResult
            {
                Id = 1, UserId = userId, Name = "Setlist 1", CreatedAt = DateTime.Now,
                SongId = 2, SongOrder = 2, Notes = "Note 2", Title = "Song 2", Key = "D", BPM = 130, DurationMinutes = 4
            }
        };

        _mockDbContext.Setup(db => db.LoadData<SetlistSongResult, dynamic>(
            It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns(setlistData);

        var result = _repo.GetSetlistWithSongs(id);

        Assert.NotNull(result);
        Assert.Equal(2, result.SetlistSongs.Count);
        Assert.Equal("Song 1", result.SetlistSongs[0].Song.Title);
        Assert.Equal("Song 2", result.SetlistSongs[1].Song.Title);
    }

    [Fact]
    public void GetUserSetlists_ShouldReturnCorrectSetlistSummaries()
    {
        var userId = Guid.NewGuid();
        var setlists = new List<SetlistSummary>
        {
            new SetlistSummary { Id = 1, Title = "Rock Set", CreatedAt = new DateTime(2024, 1, 1), SongCount = 3 },
            new SetlistSummary { Id = 2, Title = "Acoustic Vibes", CreatedAt = new DateTime(2024, 2, 1), SongCount = 5 },
            new SetlistSummary { Id = 3, Title = "Empty Set", CreatedAt = new DateTime(2024, 3, 1), SongCount = 0 }
        };

        _mockDbContext.Setup(db => db.LoadData<SetlistSummary, dynamic>(
            It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>(), false))
            .Returns(setlists);

        var result = _repo.GetSetlistsForUser(userId);

        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Contains(result, s => s.Title == "Rock Set");
        Assert.Contains(result, s => s.Title == "Acoustic Vibes");
        Assert.Contains(result, s => s.Title == "Empty Set" && s.SongCount == 0);
    }

    [Fact]
    public void Delete_ShouldRemoveSetlist()
    {
        var id = 1;

        _repo.Delete(id);

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
