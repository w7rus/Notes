using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotesWebAPI.Data;
using NotesWebAPI.Services.Users;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _users;

        public AuthController(IUsersService depUsers)
        {
            _users = depUsers;
        }

        // Send JWT token on successful login data
        [HttpPost, Route("login")]
        [AllowAnonymous]
        public IActionResult Login([Required][FromBody] Models.View.AuthLoginModel model)
        {
            return _users.loginUser(model);
        }

        // Add new user to the database
        [HttpPost, Route("register")]
        [AllowAnonymous]
        public IActionResult Register([Required][FromBody] Models.View.AuthRegisterModel model)
        {
            return _users.registerUser(model);
        }
    }
}