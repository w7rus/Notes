using System.Collections.Generic;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Notes
{
    public interface INotesRepository
    {
        public IEnumerable<Note> GetNotes(int userId);
        public Note GetNote(int noteId);
        public Note AddNote(Note note);
        public void UpdateNote(Note note);
        public void DeleteNote(Note note);

        public IEnumerable<Note> GetSharedNotes(int userId);
        public Note GetSharedNote(int noteId);
        public void UpdateSharedNote(Note note);
    }
}
