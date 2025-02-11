using App.Models;

namespace App.Repo
{
    public interface ISetlistRepo
    {
        int Create(Setlist setlist);
        IEnumerable<Setlist> GetAll();
        Setlist GetById(int id);
        Setlist GetSetlistWithSongs(int id);
        void Update(Setlist setlist);
        void Delete(int id);
    }
}