using App.Data.Context;
using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repo
{
    public class PracticeSessionRepo
    {
        private readonly ISqlDbContext _db;

        public PracticeSessionRepo(ISqlDbContext db)
        {
            _db = db;
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PracticeSession> GetAll()
        {
            var sql = @"SELECT * FROM PracticeSessions;";
            return _db.LoadData<PracticeSession, dynamic>(sql, new { }, "Default");
        }

        public void GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
