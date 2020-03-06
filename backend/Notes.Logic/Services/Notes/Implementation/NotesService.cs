using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<Note> ListNotes(int userId)
        {
            return _notesRepository.GetNotes(userId);
        }

        public IEnumerable<Note> ListNotes(int userId, string search, int sorting, int display, int page)
        {
            var notes = _notesRepository.GetNotes(userId);

            //Sorting
            switch (sorting)
            {
                default:
                    notes = notes.OrderBy(n => n.Title);
                    break;

                case 0:
                    notes = notes.OrderBy(n => n.Title);
                    break;

                case 1:
                    notes = notes.OrderByDescending(n => n.Title);
                    break;
            }

            //Search

            if (!string.IsNullOrEmpty(search))
                notes = notes.Where(n => n.Title.Contains(search));

            //Pagination

            var notesCount = Convert.ToInt32(Math.Ceiling(notes.Count() / (decimal)display));

            notes = notes.Skip(display * (page)).Take(display);

            return notes;
        }

        public Note GetNote(int noteId)
        {
            return _notesRepository.GetNote(noteId);
        }

        public Note AddNote(string title, string body, int userId, IEnumerable<SharingData> sharingUsersData)
        {
            var note = new Note
            {
                UserId = userId,
                Title = title,
                Body = body
            };

            var addedNote = _notesRepository.AddNote(note);
            _sharesService.AddShares(addedNote.Id, sharingUsersData);

            return note;
        }

        public void UpdateNote(int noteId, string title, string body, int userId, IEnumerable<SharingData> sharingUsersData)
        {
            var note = _notesRepository.GetNote(noteId);

            if (note == null)
                throw new ArgumentException($"Note[{noteId}] does not exists!");

            if (note.UserId != userId)
                throw new InvalidOperationException($"User[{userId}] does not have permissions to operate with note[{note.UserId}]");

            note.Title = title;
            note.Body = body;

            _notesRepository.UpdateNote(note);
            _sharesService.UpdateShares(noteId, sharingUsersData);
        }

        public void DeleteNote(int noteId, int userId)
        {
            var note = _notesRepository.GetNote(noteId);

            if (note == null)
                throw new ArgumentException($"Note[{noteId}] does not exists!");

            if (note.UserId != userId)
                throw new InvalidOperationException($"User[{userId}] does not have permissions to operate with note[{note.UserId}]");

            _sharesService.DeleteShares(noteId);
            _notesRepository.DeleteNote(note);
        }

        public int GetNoteCount(int userId)
        {
            return _notesRepository.GetNotes(userId).Count();
        }

        public int GetNoteCount(int userId, string search)
        {
            var notes = _notesRepository.GetNotes(userId);

            //Search

            if (!string.IsNullOrEmpty(search))
                notes = notes.Where(n => n.Title.Contains(search));

            return notes.Count();
        }
        #endregion

        #region Shared
        public IEnumerable<Note> ListSharedNotes(int userId)
        {
            return _notesRepository.GetSharedNotes(userId);
        }

        public IEnumerable<Note> ListSharedNotes(int userId, string search, int sorting, int display, int page)
        {
            var notes = _notesRepository.GetSharedNotes(userId);

            //Sorting
            switch (sorting)
            {
                default:
                    notes = notes.OrderBy(n => n.Title);
                    break;

                case 0:
                    notes = notes.OrderBy(n => n.Title);
                    break;

                case 1:
                    notes = notes.OrderByDescending(n => n.Title);
                    break;
            }

            //Search

            if (!string.IsNullOrEmpty(search))
                notes = notes.Where(n => n.Title.Contains(search));

            //Pagination

            var notesCount = Convert.ToInt32(Math.Ceiling(notes.Count() / (decimal)display));

            notes = notes.Skip(display * (page)).Take(display);

            return notes;
        }

        public Note GetSharedNote(int noteId)
        {
            return _notesRepository.GetSharedNote(noteId);
        }

        public void UpdateSharedNote(int noteId, string title, string body, int userId, IEnumerable<SharingData> sharingUsersData)
        {
            var note = _notesRepository.GetSharedNote(noteId);

            if (note == null)
                throw new ArgumentException($"Note[{noteId}] does not exists!");

            var sharedUsersData = _sharesService.GetShares(noteId);

            var sharedUserData = sharedUsersData.FirstOrDefault(s => s.UserId == userId);

            if (sharedUserData == null)
                throw new InvalidOperationException($"User[{userId}] does not have permissions to operate with note[{note.UserId}]");

            if (sharedUserData.Level < SharingLevels.Level.ReadWrite)
                throw new InvalidOperationException($"User[{userId}] does not have READWRITE permissions to operate with note[{note.UserId}]");

            note.Title = title;
            note.Body = body;

            _notesRepository.UpdateSharedNote(note);
        }

        public int GetSharedNoteCount(int userId)
        {
            return _notesRepository.GetSharedNotes(userId).Count();
        }

        public int GetSharedNoteCount(int userId, string search)
        {
            var notes = _notesRepository.GetSharedNotes(userId);

            //Search

            if (!string.IsNullOrEmpty(search))
                notes = notes.Where(n => n.Title.Contains(search));

            return notes.Count();
        }
        #endregion
    }
}
