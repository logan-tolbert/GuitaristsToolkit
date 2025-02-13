using App.Models;

namespace App.Repo
{
    public interface ISongRepo
    {
        int Create(Song song);
        void Delete(int id);
        IEnumerable<Song> GetAll();
        Song GetById(int id);
        void Update(Song song);
    }
}