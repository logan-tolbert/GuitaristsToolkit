using App.Data.Context;
using App.Models;


namespace App.Repo
{
    public class SongRepo : ISongRepo
    {
        private readonly ISqlDbContext _db;

        public SongRepo(ISqlDbContext db)
        {
            _db = db;
        }

        public int Create(Song song)
        {
            var sql = @"INSERT INTO Songs 
                        (Title, [Key], BPM, DurationMinutes, Notes)
                        Values (@Title, @Key, @BPM, @DurationMinutes, @Notes)
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            var id = _db.LoadData<int, dynamic>(sql, new
            {
                song.Title,
                song.Key,
                song.BPM,
                song.DurationMinutes,
                song.Notes
            }).FirstOrDefault();

            return id;
        }

        public IEnumerable<Song> GetAll()
        {
            var sql = @"SELECT * FROM Songs;";
            return _db.LoadData<Song, dynamic>(sql, new { });
        }


        public Song GetById(int id)
        {
            var sql = @"SELECT * FROM Songs WHERE Id = @Id;";

            return _db.LoadData<Song, dynamic>(sql, new { Id = id }).Single();
        }

        public void Update(Song song)
        {
            var sql = @"UPDATE Songs
                       SET Title = @Title, [Key] = @Key, BPM = @BPM,
                           DurationMinutes = @DurationMinutes, Notes = @Notes
                       WHERE Id = @Id;";

            _db.SaveData<Song, dynamic>(sql, new
            {
                song.Id,
                song.Title,
                song.Key,
                song.BPM,
                song.DurationMinutes,
                song.Notes
            });
        }

        public void Delete(int id)
        {
            var sql = @"DELETE FROM SetlistSongs WHERE SongId = @Id;
                        DELETE FROM Songs WHERE Id = @Id;";

            _db.SaveData<Song, dynamic>(sql, new { Id = id });
        }

    }
}
