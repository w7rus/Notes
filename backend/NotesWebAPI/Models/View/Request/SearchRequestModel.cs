﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotesWebAPI.Models.View.Request
{
    public class SearchRequestModel
    {
        public string Search { get; set; }
        [Required]
        public int Sorting { get; set; }
        [Required]
        public int Display { get; set; }
        [Required]
        public int Page { get; set; }
    }
}
