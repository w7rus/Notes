using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Logic.Common;
using Notes.Logic.Models;
using Notes.Logic.Services.Notes;
using Notes.Logic.Services.Shares;
using Notes.Logic.Services.Users;
using NotesWebAPI.Models.View;
using NotesWebAPI.Models.View.Request;
using Serilog;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharesController : ControllerBase
    {
        private readonly ISharesService _sharesService;
        private readonly INotesService _notesService;
        private readonly IUsersService _usersService;

        public SharesController(ISharesService sharesService
            , INotesService notesService
            , IUsersService usersService
            )
        {
            _sharesService = sharesService;
            _notesService = notesService;
            _usersService = usersService;
        }

        //Share Read[]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{noteId}")]
        public async Task<ActionResult<ICollection<ShareResult>>> GetShares([Required] int noteId)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var note = await _notesService.GetNote(noteId);

                if (note.UserId != userId)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                var shares = _sharesService.GetShares(noteId).ToList();

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending {shares.Count()} shares[...] to user[{userId}]");

                return Ok(shares.Select(s =>
                    new ShareResult
                    {
                        Username = _usersService.GetUsernameByUserId(s.UserId).Result,
                        UserId = s.UserId,
                        Level = s.Level
                    }
                ));
            }
            catch (Exception e)
            {
                LogError(e);
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("findShares/{noteId}")]
        public async Task<ActionResult<ICollection<ShareResult>>> GetSharesFiltered([Required] int noteId, [Required] DashboardShareFilterRequest modal)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var note = await _notesService.GetNote(noteId);

                if (note.UserId != userId)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                var shares = _sharesService.GetShares(noteId, modal.Display, modal.Page).ToList();

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending {shares.Count()} shares[...] to user[{userId}]");

                return Ok(shares.Select(s =>
                    new ShareResult
                    {
                        Username = _usersService.GetUsernameByUserId(s.UserId).Result,
                        UserId = s.UserId,
                        Level = s.Level
                    }
                ));
            }
            catch (Exception e)
            {
                LogError(e);
                return BadRequest(e.Message);
            }
        }

        //Share Add
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{noteId}")]
        public async Task<ActionResult> AddShare([Required] int noteId, [Required][FromBody]DashboardShareRequest model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var note = await _notesService.GetNote(noteId);

                if (note.UserId != userId)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                await _sharesService.AddShare(noteId, model.UserId, model.Level);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Successfully added share for note[{note.Id}] of user[{model.UserId}]");

                return Ok();

            }
            catch (Exception e)
            {
                LogError(e);
                return BadRequest(e.Message);
            }
        }
        
        //Share Update
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{noteId}")]
        public async Task<ActionResult> UpdateShare([Required] int noteId, [Required][FromBody]DashboardShareRequest model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var note = await _notesService.GetNote(noteId);

                if (note.UserId != userId)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                await _sharesService.UpdateShare(noteId, model.UserId, model.Level);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Successfully updated share for note[{note.Id}] of user[{model.UserId}]");

                return Ok();
            }
            catch (Exception e)
            {
                LogError(e);
                return BadRequest(e.Message);
            }
        }

        //Share Delete
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{noteId}/{incUserId}")]
        public async Task<ActionResult> DeleteShare([Required] int noteId, [Required] int incUserId)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var note = await _notesService.GetNote(noteId);

                if (note.UserId != userId)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                await _sharesService.DeleteShare(noteId, incUserId);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Successfully deleted share for note[{note.Id}] of user[{incUserId}]");

                return Ok();
            }
            catch (Exception e)
            {
                LogError(e);
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("findSharesCount/{noteId}")]
        public async Task<ActionResult<int>> GetSharesCount([Required] int noteId)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var note = await _notesService.GetNote(noteId);

                if (note.UserId != userId)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                var count = _sharesService.GetShares(noteId).ToList().Count;

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending {count} int to user[{userId}]");

                return Ok(count);
            }
            catch (Exception e)
            {
                LogError(e);
                return BadRequest(e.Message);
            }
        }



        //Other
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("findUsers/{noteId}")]
        public async Task<ActionResult<UserResult>> FindUsersFiltered([Required] int noteId, [Required][FromBody] DashboardUserFilterRequest model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                // Check if the user is a owner of requested Note
                var note = await _notesService.GetNote(noteId);
                if (note.UserId != userId)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                // Get all shares already available for requested Note
                var shares = _sharesService.GetShares(noteId).ToList();

                // Get all users matching search model
                var users = (await _usersService.ListUsers(model.Search, model.Sorting, model.Display, model.Page))
                    .ToList();

                // Filter out users that are already in Shares
                users = users.Where(u => shares.FirstOrDefault(s => s.UserId == u.Id) == null && u.Id != note.UserId && u.Id != 1).ToList();

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending {users.Count()} users[{string.Join(", ", users.Select(n => n.Id))}] filters:[\"{model.Search}\", {model.Sorting}, {model.Display}, {model.Page}] to user[{userId}]");

                return Ok(users.Select(u => new UserResult
                {
                    Id = u.Id,
                    Username = u.Username
                }).ToArray());
            }
            catch (Exception e)
            {
                LogError(e);
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("findUsersCount/{noteId}")]
        public async Task<ActionResult<int>> FindUsersCount([Required] int noteId)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                // Check if the user is a owner of requested Note
                var note = await _notesService.GetNote(noteId);
                if (note.UserId != userId)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                // Get all shares already available for requested Note
                var shares = _sharesService.GetShares(noteId).ToList();

                // Get all users matching search model
                var users = (await _usersService.ListUsers())
                    .ToList();

                // Filter out users that are already in Shares
                var count = users.Where(u => shares.FirstOrDefault(s => s.UserId == u.Id) == null).ToList().Count();

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending int[{count}] to user[{userId}]");

                return Ok(count);
            }
            catch (Exception e)
            {
                LogError(e);
                return BadRequest(e.Message);
            }
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("findUsersFilteredCount/{noteId}")]
        public async Task<ActionResult<int>> FindUsersFilteredCount([Required] int noteId, [Required][FromBody] DashboardUserFilterRequest model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                // Check if the user is a owner of requested Note
                var note = await _notesService.GetNote(noteId);
                if (note.UserId != userId)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                // Get all shares already available for requested Note
                var shares = _sharesService.GetShares(noteId).ToList();

                // Get all users matching search model
                var users = (await _usersService.ListUsers(model.Search, model.Sorting, model.Display, model.Page))
                    .ToList();

                // Filter out users that are already in Shares
                var count = users.Where(u => shares.FirstOrDefault(s => s.UserId == u.Id) == null && u.Id != note.UserId && u.Id != 1).ToList().Count();

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending int[{count}] to user[{userId}]");

                return Ok(count);
            }
            catch (Exception e)
            {
                LogError(e);
                return BadRequest(e.Message);
            }
        }

        #region Helpers
        private void LogError(Exception e)
        {
            Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
        }
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