using System.Collections.Generic;
using System.Linq;
using Notes.Logic.Common;
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

        #region nonShared
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
        #endregion

        #region Shared
        public IEnumerable<Note> GetSharedNotes(int userId)
        {
            var shares = _context.Sharing.Where(s => s.UserId == userId);

            var notes = new List<Note>();
            foreach (var share in shares)
            {
                if (share.Level >= SharingLevels.Level.Read)
                    notes.Add(_context.Notes.Find(share.NoteId));
            }
            return notes.ToArray();
        }

        public Note GetSharedNote(int noteId)
        {
            return _context.Notes.Find(noteId);
        }

        public void UpdateSharedNote(Note note)
        {
            _context.Notes.Update(note);
            _context.SaveChanges();
        }
        #endregion
    }
}
