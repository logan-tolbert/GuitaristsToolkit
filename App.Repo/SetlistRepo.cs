using App.Data.Context;
using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Repo
{
    public class SetlistRepo : ISetlistRepo
    {
        private readonly ISqlDbContext _db;

        public SetlistRepo(ISqlDbContext db)
        {
            _db = db;
        }

        public int Create(Setlist setlist)
        {
            var sql = @"INSERT INTO Setlists 
                        (UserId, Name, CreatedAt)
                        VALUES (@UserId, @Name, @CreatedAt);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            var id = _db.LoadData<int, dynamic>(sql, new
            {
                UserId = setlist.UserId,
                setlist.Name,
                setlist.CreatedAt
            }).FirstOrDefault();

            return id;
        }

        public IEnumerable<Setlist> GetAll()
        {
            var sql = @"SELECT * FROM Setlists;";
            return _db.LoadData<Setlist, dynamic>(sql, new { });
        }

        public Setlist GetById(int id)
        {
            var sql = @"SELECT * FROM Setlists WHERE Id = @Id;";
            return _db.LoadData<Setlist, dynamic>(sql, new { Id = id }).Single();
        }

        public Setlist GetSetlistWithSongs(int id)
        {
            var sql = @"SELECT s.Id, s.UserId, s.Name, s.CreatedAt, 
                   ss.SongId, ss.SongOrder, 
                   so.Id AS SongId, so.Title, so.[Key], so.BPM, so.DurationMinutes, so.Notes
                FROM Setlists s
                LEFT JOIN SetlistSongs ss ON s.Id = ss.SetlistId
                LEFT JOIN Songs so ON ss.SongId = so.Id
                WHERE s.Id = @Id;";

            var result = _db.LoadData<SetlistSongResult, dynamic>(sql, new { Id = id });

            var setlistDictionary = new Dictionary<int, Setlist>();

            foreach (var row in result)
            {
                if (!setlistDictionary.TryGetValue(row.Id, out var setlist))
                {
                    setlist = new Setlist
                    {
                        Id = row.Id,
                        UserId = row.UserId,
                        Name = row.Name,
                        CreatedAt = row.CreatedAt,
                        SetlistSongs = new List<SetlistSong>()
                    };

                    setlistDictionary[row.Id] = setlist;
                }

                if (row.SongId.HasValue)
                {
                    var setlistSong = new SetlistSong
                    {
                        SetlistId = row.Id,
                        SongId = row.SongId.Value,
                        SongOrder = row.SongOrder ?? 0,
                        Song = new Song
                        {
                            Id = row.SongId.Value,
                            Title = row.Title ?? "Unknown",
                            Key = row.Key ?? "N/A",
                            BPM = row.BPM ?? 0,
                            DurationMinutes = row.DurationMinutes ?? 0,
                            Notes = row.Notes ?? string.Empty
                        }
                    };

                    setlist.SetlistSongs.Add(setlistSong);
                }
            }

            return setlistDictionary.Values.First();
        }



        public IEnumerable<SetlistSummary> GetSetlistsForUser(Guid userId)
        {
            var sql = @"
                        SELECT s.Id, s.Name AS Title, COUNT(ss.SongId) AS SongCount
                        FROM Setlists s
                        LEFT JOIN SetlistSongs ss ON s.Id = ss.SetlistId
                        WHERE s.UserId = @UserId
                        GROUP BY s.Id, s.Name;";

            return _db.LoadData<SetlistSummary, dynamic>(sql, new { UserId = userId });
        }

        public void AddSongToSetlist(SetlistSong setlistSong)
        {
            var sqlOrder = @"SELECT COALESCE(MAX(SongOrder), 0) + 1 FROM SetlistSongs WHERE SetlistId = @SetlistId;";
            int nextOrder = _db.LoadData<int, dynamic>(sqlOrder, new { setlistSong.SetlistId }).FirstOrDefault();

            if (nextOrder == 0)
            {
                nextOrder = 1;
            }

            var sql = @"INSERT INTO SetlistSongs (SetlistId, SongId, SongOrder, Notes)
                VALUES (@SetlistId, @SongId, @SongOrder, @Notes);";

            _db.SaveData<SetlistSong, dynamic>(sql, new
            {
                setlistSong.SetlistId,
                setlistSong.SongId,
                SongOrder = nextOrder,
                setlistSong.Notes
            });
        }

        public void Delete(int id)
        {
            var sql = @" DELETE FROM SetlistSongs WHERE SetlistId = @id;
                         DELETE FROM Setlists WHERE Id = @id;";

            _db.SaveData<dynamic, dynamic>(sql, new { id = id });
        }

    }
}
