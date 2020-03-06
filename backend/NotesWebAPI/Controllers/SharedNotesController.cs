using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Logic.Common;
using Notes.Logic.Models;
using Notes.Logic.Services.Notes;
using Notes.Logic.Services.Shares;
using Notes.Logic.Services.Users;
using NotesWebAPI.Models.View.Request;
using Serilog;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedNotesController : ControllerBase
    {
        private readonly INotesService _notesService;
        private readonly ISharesService _sharesService;
        private readonly IUsersService _usersService;

        public SharedNotesController(INotesService notesService
            , ISharesService sharesService
            , IUsersService usersService
        )
        {
            _notesService = notesService;
            _sharesService = sharesService;
            _usersService = usersService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteResult>>> SharedNoteList()
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var notes = (await _notesService.ListSharedNotes(userId)).ToList();

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending {notes.Count()} shared notes[{string.Join(", ", notes.Select(n => n.Id))}] to user[{userId}]");

                return new ActionResult<IEnumerable<NoteResult>>(notes.Select(n =>
                    new NoteResult
                    {
                        Body = n.Body,
                        Id = n.Id,
                        Title = n.Title,
                        SharedUsersData = _sharesService.GetShares(n.Id).Where(s => s.UserId == userId).Select(s => new SharingData()
                        {
                            Level = s.Level,
                            UserId = s.UserId,
                            Username = s.User.Username//_usersService.GetUsernameByUserId(s.UserId) 
                        })
                    }
                ));
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("list")]
        public async Task<ActionResult<IEnumerable<NoteResult>>> SharedNoteListFiltered([Required][FromBody] SearchRequestModel model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var notes = (await _notesService.ListSharedNotes(userId, model.Search, model.Sorting, model.Display, model.Page)).ToList();

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending {notes.Count()} shared notes[{string.Join(", ", notes.Select(n => n.Id))}] filters:[\"{model.Search}\", {model.Sorting}, {model.Display}, {model.Page}] to user[{userId}]");

                return new ActionResult<IEnumerable<NoteResult>>(notes.Select(n =>
                    new NoteResult
                    {
                        Body = n.Body,
                        Id = n.Id,
                        Title = n.Title,
                        SharedUsersData = _sharesService.GetShares(n.Id).Where(s => s.UserId == userId).Select(s => new SharingData()
                        {
                            Level = s.Level,
                            UserId = s.UserId,
                            Username = s.User.Username//_usersService.GetUsernameByUserId(s.UserId)
                        })
                    }
                ));
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{noteId}")]
        public async Task<ActionResult<NoteResult>> GetSharedNote([Required] int noteId)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var note = await _notesService.GetSharedNote(noteId);

                var sharedUsersData = _sharesService.GetShares(noteId);

                var sharedUserData = sharedUsersData.FirstOrDefault(s => s.UserId == userId);

                if (sharedUserData == null)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                if (sharedUserData.Level < SharingLevels.Level.Read)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have READ/READWRITE permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending shared note[{note.Id}] to user[{userId}]");

                return new NoteResult
                {
                    Body = note.Body,
                    Id = note.Id,
                    Title = note.Title,
                    SharedUsersData = _sharesService.GetShares(note.Id).Where(s => s.UserId == userId).Select(s => new SharingData()
                    {
                        Level = s.Level,
                        UserId = s.UserId,
                        Username = s.User.Username//_usersService.GetUsernameByUserId(s.UserId)
                    })
                };
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateSharedNote([Required] int noteId, [Required][FromBody] NoteRequestModel model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                await _notesService.UpdateSharedNote(noteId, model.Title, model.Body, userId, model.SharedUsersData);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Successfully updated shared note[{noteId}] of user[{userId}]");

                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("count")]
        public async Task<IActionResult> SharedNoteCount()
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var count = await _notesService.GetSharedNoteCount(userId);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending int[{count}] to user[{userId}]");

                return Ok(count);
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("countFiltered")]
        public async Task<IActionResult> SharedNoteCountFiltered([Required] [FromBody] SearchRequestModel model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var count = await _notesService.GetSharedNoteCount(userId, model.Search);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending int[{count}] filters:[\"{model.Search}\"] to user[{userId}]");

                return Ok(count);
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