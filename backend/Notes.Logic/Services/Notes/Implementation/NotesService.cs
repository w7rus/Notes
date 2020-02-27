using System;
using System.Collections.Generic;
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
    }
}
