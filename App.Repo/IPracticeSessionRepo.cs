using App.Models;

namespace App.Repo
{
    public interface IPracticeSessionRepo
    {
        void Create(PracticeSession session);
        void Delete(int id);
        IEnumerable<PracticeSession> GetAll();
        IEnumerable<PracticeSession> GetById(int id);
        void Update(PracticeSession session);
    }
}