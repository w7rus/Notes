using System.Collections.Generic;
using System.Threading.Tasks;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Notes
{
    public interface INotesRepository
    {
        #region notShared
        public Task<ICollection<Note>> GetNotes(int userId);
        public Task<Note> GetNote(int noteId);
        public Task<Note> AddNote(Note note);
        public Task UpdateNote(Note note);
        public Task DeleteNote(Note note);
        #endregion

        #region Shared
        public Task<ICollection<Note>> GetSharedNotes(int userId);
        public Task<Note> GetSharedNote(int noteId);
        public Task UpdateSharedNote(Note note);
        #endregion
    }
}
