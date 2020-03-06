using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<Note>> GetNotes(int userId)
        {
            return await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
        }

        public async Task<Note> GetNote(int noteId)
        {
            return await _context.Notes.FindAsync(noteId);
        }

        public async Task<Note> AddNote(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();

            return note;
        }

        public async Task UpdateNote(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNote(Note note)
        {
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Shared
        public async Task<IEnumerable<Note>> GetSharedNotes(int userId)
        {
            var shares = _context.Sharing.Where(s => s.UserId == userId);

            var notes = new List<Note>();
            foreach (var share in shares)
            {
                if (share.Level >= SharingLevels.Level.Read)
                    notes.Add(await _context.Notes.FindAsync(share.NoteId));
            }
            return notes.ToArray();
        }

        public async Task<Note> GetSharedNote(int noteId)
        {
            return await _context.Notes.FindAsync(noteId);
        }

        public async Task UpdateSharedNote(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
