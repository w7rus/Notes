using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Notes.Logic.Common;

namespace NotesWebAPI.Models.View.Request
{
    public class ShareResult
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public SharingLevels.Level Level { get; set; }
    }
}
