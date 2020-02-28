using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Logic.Models;
using Notes.Logic.Services.Users;
using NotesWebAPI.Models.View.Request;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public AuthController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // Send JWT token on successful login data
        [HttpPost, Route("login")]
        [AllowAnonymous]
        public ActionResult<LoginResult> Login([Required][FromBody] LoginRequestModel model)
        {
            try
            {
                return Ok(_usersService.LoginUser(model.Username, model.Password));
            }
            catch (Exception e)
            {
                //TODO: add Logs
                return BadRequest(e.Message);
            }
        }

        // Add new user to the database
        [HttpPost, Route("register")]
        [AllowAnonymous]
        public IActionResult Register([Required][FromBody] RegisterRequestModel model)
        {
            try
            {
                _usersService.RegisterUser(model.Username, model.Password, model.PasswordRepeat);
                return Ok();
            }
            catch (Exception e)
            {
                //TODO: add Logs
                return BadRequest(e.Message);
            }
        }
    }
}