using System;
using System.Collections.Generic;
using System.Linq;
using Notes.Logic.Models.Database;
using Notes.Logic.Repositories.Notes;

namespace Notes.Logic.Services.Notes.Implementation
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository _notesRepository;

        public NotesService(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;
        }

        public IEnumerable<Note> ListNotes(int userid)
        {
            return _notesRepository.GetNotes(userid);
        }

        public IEnumerable<Note> ListNotes(int userid, string search, int sorting, int display, int page)
        {
            var notes = _notesRepository.GetNotes(userid);

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

        public Note GetNote(int noteid)
        {
            return _notesRepository.GetNote(noteid);
        }

        public Note AddNote(string title, string body, int userid)
        {
            var note = new Note
            {
                UserId = userid,
                Title = title,
                Body = body
            };

            _notesRepository.AddNote(note);

            return note;
        }

        public void UpdateNote(int noteid, string title, string body, int userid)
        {
            var note = _notesRepository.GetNote(noteid);

            if (note == null)
                throw new ArgumentException("Invalid note data!");

            if (note.UserId != userid)
                throw new InvalidOperationException("User does not own this note!");

            note.Title = title;
            note.Body = body;

            _notesRepository.UpdateNote(note);
        }

        public void DeleteNote(int noteid, int userid)
        {
            var note = _notesRepository.GetNote(noteid);

            if (note == null)
                throw new ArgumentException("Invalid note data!");

            if (note.UserId != userid)
                throw new InvalidOperationException("User does not own this note!");

            _notesRepository.DeleteNote(note);
        }

        public int GetNoteCount(int userid)
        {
            return _notesRepository.GetNotes(userid).Count();
        }

        public int GetNoteCount(int userid, string search, int sorting)
        {
            var notes = _notesRepository.GetNotes(userid);

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

            return notes.Count();
        }
    }
}
