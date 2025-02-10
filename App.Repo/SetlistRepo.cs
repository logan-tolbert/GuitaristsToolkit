using App.Data.Context;
using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repo
{
    public class SetlistRepo
    {
        private readonly ISqlDbContext _db;

        public SetlistRepo(ISqlDbContext db)
        {
            _db = db;
        }

        public int Create(Setlist setList)
        {
            var sql = @"INSERT INTO SetLists 
                        (UserId, Name, CreatedAt)
                        VALUES (@UserId, @Name, @CreatedAt);";

            _db.SaveData<Setlist, dynamic>(sql, new
            {
                setList.UserId,
                setList.Name,
                setList.CreatedAt
            });
        }
    }
}
