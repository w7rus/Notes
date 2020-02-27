﻿using System.ComponentModel.DataAnnotations;

namespace NotesWebAPI.Models.View.Request
{
    public class LoginRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}