using System.Collections.Generic;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Services.Notes
{
    public interface INotesService
    {
        public IEnumerable<Note> ListNotes(int userid);
        public IEnumerable<Note> ListNotes(int userid, string search, int sorting, int display, int page);
        public Note GetNote(int noteid);
        public Note AddNote(string title, string body, int userid);
        public void UpdateNote(int noteid, string title, string body, int userid);
        public void DeleteNote(int noteid, int userid);

        public int GetNoteCount(int userid);
        public int GetNoteCount(int userid, string search, int sorting);
    }
}
