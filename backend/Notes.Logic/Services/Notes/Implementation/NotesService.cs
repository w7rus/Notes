using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notes.Logic.Common;
using Notes.Logic.Models.Database;
using Notes.Logic.Repositories.Notes;
using Notes.Logic.Repositories.Shares;
using Notes.Logic.Services.Shares;
using NotesWebAPI.Models.View.Request;

namespace Notes.Logic.Services.Notes.Implementation
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository _notesRepository;
        private readonly ISharesService _sharesService;

        public NotesService(INotesRepository notesRepository, ISharesService sharesService)
        {
            _notesRepository = notesRepository;
            _sharesService = sharesService;
        }

        #region nonShared
        public async Task<ICollection<Note>> ListNotes(int userId)
        {
            return await _notesRepository.GetNotes(userId);
        }

        public async Task<ICollection<Note>> ListNotes(int userId, string search, int sorting, int display, int page)
        {
            IEnumerable<Note> notes = await _notesRepository.GetNotes(userId);

            //Sorting
            notes = sorting switch
            {
                0 => notes.OrderBy(n => n.Title),
                1 => notes.OrderByDescending(n => n.Title),
                _ => notes.OrderBy(n => n.Title),
            };

            //Search

            if (!string.IsNullOrEmpty(search))
                notes = notes.Where(n => n.Title.Contains(search));

            //Pagination

            var notesCount = Convert.ToInt32(Math.Ceiling(notes.Count() / (decimal)display));

            notes = notes.Skip(display * (page)).Take(display);

            return notes.ToArray();
        }

        public async Task<Note> GetNote(int noteId)
        {
            return await _notesRepository.GetNote(noteId);
        }

        public async Task<Note> AddNote(string title, string body, int userId)
        {
            var note = new Note
            {
                UserId = userId,
                Title = title,
                Body = body
            };

            await _notesRepository.AddNote(note);

            return note;
        }

        public async Task UpdateNote(int noteId, string title, string body, int userId)
        {
            var note = await _notesRepository.GetNote(noteId);

            if (note == null)
                throw new ArgumentException($"Note[{noteId}] does not exists!");

            if (note.UserId != userId)
                throw new InvalidOperationException($"User[{userId}] does not have permissions to operate with note[{note.Id}]");

            note.Title = title;
            note.Body = body;

            await _notesRepository.UpdateNote(note);
        }

        public async Task DeleteNote(int noteId, int userId)
        {
            var note = await _notesRepository.GetNote(noteId);

            if (note == null)
                throw new ArgumentException($"Note[{noteId}] does not exists!");

            if (note.UserId != userId)
                throw new InvalidOperationException($"User[{userId}] does not have permissions to operate with note[{note.Id}]");

            await _sharesService.DeleteShares(noteId);
            await _notesRepository.DeleteNote(note);
        }

        public async Task<int> GetNoteCount(int userId)
        {
            return (await _notesRepository.GetNotes(userId)).Count();
        }

        public async Task<int> GetNoteCount(int userId, string search)
        {
            IEnumerable<Note> notes = await _notesRepository.GetNotes(userId);

            //Search

            if (!string.IsNullOrEmpty(search))
                notes = notes.Where(n => n.Title.Contains(search));

            return notes.ToArray().Count();
        }
        #endregion

        #region Shared
        public async Task<ICollection<Note>> ListSharedNotes(int userId)
        {
            return await _notesRepository.GetSharedNotes(userId);
        }

        public async Task<ICollection<Note>> ListSharedNotes(int userId, string search, int sorting, int display, int page)
        {
            IEnumerable<Note> notes = await _notesRepository.GetSharedNotes(userId);

            //Sorting
            notes = sorting switch
            {
                0 => notes.OrderBy(n => n.Title),
                1 => notes.OrderByDescending(n => n.Title),
                _ => notes.OrderBy(n => n.Title),
            };

            //Search

            if (!string.IsNullOrEmpty(search))
                notes = notes.Where(n => n.Title.Contains(search));

            //Pagination

            var notesCount = Convert.ToInt32(Math.Ceiling(notes.Count() / (decimal)display));

            notes = notes.Skip(display * (page)).Take(display);

            return notes.ToArray();
        }

        public async Task<Note> GetSharedNote(int noteId)
        {
            return await _notesRepository.GetSharedNote(noteId);
        }

        public async Task UpdateSharedNote(int noteId, string title, string body, int userId)
        {
            var note = await _notesRepository.GetSharedNote(noteId);

            if (note == null)
                throw new ArgumentException($"Note[{noteId}] does not exists!");

            var sharedUsersData = _sharesService.GetShares(noteId);

            var sharedUserData = sharedUsersData.FirstOrDefault(s => s.UserId == userId);

            if (sharedUserData == null)
                throw new InvalidOperationException($"User[{userId}] does not have permissions to operate with note[{note.Id}]");

            if (sharedUserData.Level < SharingLevels.Level.ReadWrite)
                throw new InvalidOperationException($"User[{userId}] does not have READWRITE permissions to operate with note[{note.Id}]");

            note.Title = title;
            note.Body = body;

            await _notesRepository.UpdateSharedNote(note);
        }

        public async Task<int> GetSharedNoteCount(int userId)
        {
            return (await _notesRepository.GetSharedNotes(userId)).Count();
        }

        public async Task<int> GetSharedNoteCount(int userId, string search)
        {
            IEnumerable<Note> notes = await _notesRepository.GetSharedNotes(userId);

            //Search

            if (!string.IsNullOrEmpty(search))
                notes = notes.Where(n => n.Title.Contains(search));

            return notes.ToArray().Count();
        }
        #endregion
    }
}
