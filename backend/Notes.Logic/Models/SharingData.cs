using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Notes.Logic.Common;

namespace NotesWebAPI.Models.View.Request
{
    public class SharingData
    {
        public string Username { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public SharingLevels.Level Level { get; set; }
    }
}
