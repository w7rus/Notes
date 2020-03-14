﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotesWebAPI.Models.View
{
    public class DashboardUserFilterRequest
    {
        public string Search { get; set; }
        public int Sorting { get; set; }
        public int Display { get; set; }
        public int Page { get; set; }
    }
}
