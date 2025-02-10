using App.Data.Context;
using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
#pragma warning disable CS8603 // Possible null reference return.
            return _db.LoadData<Setlist, dynamic>(sql, new { Id = id }).SingleOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
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
