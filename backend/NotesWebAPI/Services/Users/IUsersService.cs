using Microsoft.AspNetCore.Mvc;
using NotesWebAPI.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesWebAPI.Services.Users
{
    public interface IUsersService
    {
        IActionResult loginUser(AuthLoginModel model);
        IActionResult registerUser(AuthRegisterModel model);
    }
}
