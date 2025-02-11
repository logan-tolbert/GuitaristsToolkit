using App.Data.Context;
using App.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repo
{
    public partial class SetlistRepo : ISetlistRepo
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
                setlist.UserId,
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
                            ss.SongId, ss.SongOrder, ss.Notes, 
                            so.Id AS SongId, so.Title, so.Key, so.BPM, so.DurationMinutes
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
                    setlist.SetlistSongs.Add(new SetlistSong
                    {
                        SetlistId = row.Id,
                        SongId = row.SongId.Value,
                        SongOrder = row.SongOrder ?? 0,
                        Notes = row.Notes ?? string.Empty,
                        Song = new Song
                        {
                            Id = row.SongId.Value,
                            Title = row.Title ?? "Unknown",
                            Key = row.Key ?? "N/A",
                            BPM = row.BPM ?? 0,
                            DurationMinutes = row.DurationMinutes ?? 0
                        }
                    });
                }
            }

            return setlistDictionary.Values.First();
        }



        //TODO: Implement Update method and tests
        public void Update(Setlist setlist)
        {
            var sql = @"";

            _db.SaveData<Setlist, dynamic>(sql, new
            {

            });

            throw new NotImplementedException();
        }


        public void Delete(int id)
        {
            var sql = @"DELETE FROM Setlists WHERE Id = @Id;";
            _db.SaveData<Setlist, dynamic>(sql, new { Id = id });
        }
    }
}
