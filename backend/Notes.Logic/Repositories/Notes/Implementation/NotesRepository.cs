using System.Collections.Generic;
using System.Linq;
using Notes.Logic.Data;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Notes.Implementation
{
    public class NotesRepository : INotesRepository
    {
        private readonly NotesWebAPIContext _context;

        public NotesRepository(NotesWebAPIContext context)
        {
            _context = context;
        }

        public IEnumerable<Note> GetNotes(int userid)
        {
            return _context.Notes.Where(n => n.UserId == userid).ToList();
        }

        public Note GetNote(int noteid)
        {
            return _context.Notes.Find(noteid);
        }

        public void AddNote(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public void UpdateNote(Note note)
        {
            _context.Notes.Update(note);
            _context.SaveChanges();
        }

        public void DeleteNote(Note note)
        {
            _context.Notes.Remove(note);
            _context.SaveChanges();
        }
    }
}
