using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Notes.Logic.Models.Database
{
    public enum Level
    {
        Read,
        Write,
        ReadWrite,
    }

    public class SharingProps
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int NoteId { get; set; }
        public Note Note { get; set; }
        public Level Level { get; set; }
    }
}
