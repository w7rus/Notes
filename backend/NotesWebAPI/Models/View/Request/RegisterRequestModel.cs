using System.ComponentModel.DataAnnotations;

namespace NotesWebAPI.Models.View.Request
{
    public class RegisterRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordRepeat { get; set; }
    }
}
