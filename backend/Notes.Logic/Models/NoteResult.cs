using System;
using System.Collections.Generic;
using System.Text;
using NotesWebAPI.Models.View.Request;

namespace Notes.Logic.Models
{
    public class NoteResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
