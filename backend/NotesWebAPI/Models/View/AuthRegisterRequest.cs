using System.ComponentModel.DataAnnotations;

namespace NotesWebAPI.Models.View.Request
{
    public class AuthRegisterRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordRepeat { get; set; }
    }
}
