using System.Collections.Generic;
using System.Threading.Tasks;
using Notes.Logic.Models.Database;
using NotesWebAPI.Models.View.Request;

namespace Notes.Logic.Services.Notes
{
    public interface INotesService
    {
        #region notShared
        public Task<ICollection<Note>> ListNotes(int userId);
        public Task<ICollection<Note>> ListNotes(int userId, string search, int sorting, int display, int page);
        public Task<Note> GetNote(int noteId);
        public Task<Note> AddNote(string title, string body, int userId);
        public Task UpdateNote(int noteId, string title, string body, int userId);
        public Task DeleteNote(int noteId, int userId);
        public Task<int> GetNoteCount(int userId);
        public Task<int> GetNoteCount(int userId, string search);
        #endregion

        #region Shared
        public Task<ICollection<Note>> ListSharedNotes(int userId);
        public Task<ICollection<Note>> ListSharedNotes(int userId, string search, int sorting, int display, int page);
        public Task<Note> GetSharedNote(int noteId);
        public Task UpdateSharedNote(int noteId, string title, string body, int userId);
        public Task<int> GetSharedNoteCount(int userId);
        public Task<int> GetSharedNoteCount(int userId, string search);
        #endregion
    }
}
