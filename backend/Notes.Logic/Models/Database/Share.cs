using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Notes.Logic.Common;

namespace Notes.Logic.Models.Database
{
    public class Share
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int NoteId { get; set; }
        public Note Note { get; set; }
        public SharingLevels.Level Level { get; set; }
    }
}
