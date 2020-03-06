using System.Collections.Generic;
using Notes.Logic.Models.Database;
using NotesWebAPI.Models.View.Request;

namespace Notes.Logic.Services.Notes
{
    public interface INotesService
    {
        #region notShared
        public IEnumerable<Note> ListNotes(int userId);
        public IEnumerable<Note> ListNotes(int userId, string search, int sorting, int display, int page);
        public Note GetNote(int noteId);
        public Note AddNote(string title, string body, int userId, IEnumerable<SharingData> sharingUsersData);
        public void UpdateNote(int noteId, string title, string body, int userId, IEnumerable<SharingData> sharingUsersData);
        public void DeleteNote(int noteId, int userId);
        public int GetNoteCount(int userId);
        public int GetNoteCount(int userId, string search);
        #endregion

        #region Shared
        public IEnumerable<Note> ListSharedNotes(int userId);
        public IEnumerable<Note> ListSharedNotes(int userId, string search, int sorting, int display, int page);
        public Note GetSharedNote(int noteId);
        public void UpdateSharedNote(int noteId, string title, string body, int userId, IEnumerable<SharingData> sharingUsersData);
        public int GetSharedNoteCount(int userId);
        public int GetSharedNoteCount(int userId, string search);
        #endregion
    }
}
