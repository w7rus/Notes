using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Notes.Logic.Models.Database
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }
        public string Filename { get; set; }
        public int NoteId { get; set; }
        public Note Note { get; set; }
    }
}
