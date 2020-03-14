using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Logic.Models.Database;
using Notes.Logic.Services.Notes;
using Notes.Logic.Services.Shares;
using Notes.Logic.Services.Users;
using NotesWebAPI.Models.View.Request;
using Serilog;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedSharesController : ControllerBase
    {
        private readonly ISharesService _sharesService;
        private readonly INotesService _notesService;
        private readonly IUsersService _usersService;

        public SharedSharesController(ISharesService sharesService
            , INotesService notesService
            , IUsersService usersService
        )
        {
            _sharesService = sharesService;
            _notesService = notesService;
            _usersService = usersService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{noteId}")]
        public ActionResult<ShareResult> GetShare([Required] int noteId)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var share = _sharesService.GetShares(noteId).FirstOrDefault(s => s.UserId == userId);
                if (share == null)
                {
                    return BadRequest();
                }

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending share to user[{userId}]");

                return Ok(
                    new ShareResult
                    {
                        Username = _usersService.GetUsernameByUserId(share.UserId).Result,
                        UserId = share.UserId,
                        Level = share.Level
                    }
                );
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        #region Helpers

        private bool TryGetUserId(out int userId)
        {
            if (!User.Claims.Any())
            {
                userId = 0;
                return false;
            }

            userId = Convert.ToInt32(User.Claims.First(c => c?.Type == "user_id").Value);
            return true;
        }

        #endregion
    }
}