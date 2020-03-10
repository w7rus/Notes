using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NotesWebAPI.Models.View.Request
{
    public class DashboardNoteRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public IEnumerable<SharingData> SharedUsersData { get; set; }
    }
}