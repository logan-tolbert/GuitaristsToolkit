using App.Data.Context;
using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repo
{
    public class PracticeSessionRepo : IPracticeSessionRepo
    {
        private readonly ISqlDbContext _db;

        public PracticeSessionRepo(ISqlDbContext db)
        {
            _db = db;
        }

        public void Create(PracticeSession session)
        {
            var sql = @"INSERT INTO PracticeSessions
                        (UserId, Date, DurationMinutes, FocusArea, Notes)
                        VALUES (@UserId, @Date, @DurationMinutes, @FocusArea, @Notes);";

            _db.SaveData<PracticeSession, dynamic>(sql, new
            {
                session.UserId,
                session.Date,
                session.DurationMinutes,
                session.FocusArea,
                session.Notes
            });
        }

        public IEnumerable<PracticeSession> GetAll()
        {
            var sql = @"SELECT * FROM PracticeSessions;";
            return _db.LoadData<PracticeSession, dynamic>(sql, new { });
        }

        public IEnumerable<PracticeSession> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PracticeSession session)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var sql = @"DELETE FROM PracticeSessions WHERE Id = @Id;";
            _db.SaveData<PracticeSession, dynamic>(sql, new { Id = id });
        }

    }
}
