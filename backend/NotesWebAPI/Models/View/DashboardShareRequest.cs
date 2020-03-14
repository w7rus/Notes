using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Notes.Logic.Common;

namespace NotesWebAPI.Models.View
{
    public class DashboardShareRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public SharingLevels.Level Level { get; set; }
    }
}
