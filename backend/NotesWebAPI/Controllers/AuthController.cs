using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Logic.Models;
using Notes.Logic.Services.Users;
using NotesWebAPI.Models.View.Request;
using Serilog;

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
        public async Task<ActionResult<LoginResult>> Login([Required][FromBody] LoginRequest model)
        {
            try
            {
                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Logging in @{model.Username}");
                var user = await _usersService.LoginUser(model.Username, model.Password);
                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Successfully logged in @{user.Username}[{user.UserID}]");
                return Ok(user);
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        // Add new user to the database
        [HttpPost, Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([Required][FromBody] AuthRegisterRequest model)
        {
            try
            {
                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Signing up @{model.Username}");
                await _usersService.RegisterUser(model.Username, model.Password, model.PasswordRepeat);
                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Successfully registered @{model.Username}");
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}