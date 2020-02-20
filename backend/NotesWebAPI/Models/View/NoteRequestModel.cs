using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesWebAPI.Models.View
{
    public class NoteRequestModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}