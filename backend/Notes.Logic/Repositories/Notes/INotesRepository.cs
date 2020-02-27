using System.Collections.Generic;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Notes
{
    public interface INotesRepository
    {
        public IEnumerable<Note> GetNotes(int userid);
        public Note GetNote(int noteid);
        public void AddNote(Note note);
        public void UpdateNote(Note note);
        public void DeleteNote(Note note);
    }
}
