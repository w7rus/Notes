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

        public IEnumerable<Note> GetNotes(int userId)
        {
            return _context.Notes.Where(n => n.UserId == userId).ToList();
        }

        public Note GetNote(int noteId)
        {
            return _context.Notes.Find(noteId);
        }

        public Note AddNote(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();

            return note;
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
