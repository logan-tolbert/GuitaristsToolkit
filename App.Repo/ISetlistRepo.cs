using App.Models;
using System;
using System.Collections.Generic;

namespace App.Repo
{
    public interface ISetlistRepo
    {
        int Create(Setlist setlist);
        IEnumerable<Setlist> GetAll();
        Setlist GetById(int id);
        Setlist GetSetlistWithSongs(int id);
        void Delete(int id);
        IEnumerable<SetlistSummary> GetSetlistsForUser(Guid userId);
        void AddSongToSetlist(SetlistSong setlistSong);
        void RemoveSongFromSetlist(int setlistId, int songId);
    }
}
