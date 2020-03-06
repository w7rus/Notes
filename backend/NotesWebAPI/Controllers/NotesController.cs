using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Logic.Services.Notes;
using NotesWebAPI.Models.View.Request;
using Notes.Logic.Models;
using Notes.Logic.Repositories.Shares;
using Notes.Logic.Services.Shares;
using Notes.Logic.Services.Users;
using Serilog;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;
        private readonly ISharesService _sharesService;
        private readonly IUsersService _usersService;

        public NotesController(INotesService notesService
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
        public ActionResult<IEnumerable<NoteResult>> NoteList()
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var notes = _notesService.ListNotes(userId);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending {notes.Count()} notes[{string.Join(", ", notes.Select(n => n.Id))}] to user[{userId}]");

                return new ActionResult<IEnumerable<NoteResult>>(notes.Select(n =>
                    new NoteResult
                    {
                        Body = n.Body,
                        Id = n.Id,
                        Title = n.Title,
                        SharedUsersData = new List<SharingData>(_sharesService.GetShares(n.Id).Select(s => new SharingData()
                        {
                            Level = s.Level,
                            UserId = s.UserId,
                            Username = _usersService.GetUsernameByUserId(s.UserId)
                        })).ToArray()
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
        public ActionResult<IEnumerable<NoteResult>> NoteListFiltered([Required][FromBody] SearchRequestModel model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var notes = _notesService.ListNotes(userId, model.Search, model.Sorting, model.Display, model.Page);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending {notes.Count()} notes[{string.Join(", ", notes.Select(n => n.Id))}] filters:[\"{model.Search}\", {model.Sorting}, {model.Display}, {model.Page}] to user[{userId}]");

                return new ActionResult<IEnumerable<NoteResult>>(notes.Select(n =>
                    new NoteResult
                    {
                        Body = n.Body,
                        Id = n.Id,
                        Title = n.Title,
                        SharedUsersData = new List<SharingData>(_sharesService.GetShares(n.Id).Select(s => new SharingData()
                        {
                            Level = s.Level,
                            UserId = s.UserId,
                            Username = _usersService.GetUsernameByUserId(s.UserId)
                        })).ToArray()
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
        public ActionResult<NoteResult> GetNote([Required] int noteId)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var note = _notesService.GetNote(noteId);

                if (note.UserId != userId)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending note[{note.Id}] to user[{userId}]");

                return new NoteResult
                {
                    Body = note.Body,
                    Id = note.Id,
                    Title = note.Title,
                    SharedUsersData = new List<SharingData>(_sharesService.GetShares(note.Id).Select(s => new SharingData()
                    {
                        Level = s.Level,
                        UserId = s.UserId,
                        Username = _usersService.GetUsernameByUserId(s.UserId)
                    })).ToArray()
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
        public IActionResult UpdateNote([Required] int noteId, [Required][FromBody] NoteRequestModel model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                _notesService.UpdateNote(noteId, model.Title, model.Body, userId, model.SharedUsersData);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Successfully updated note[{noteId}] of user[{userId}]");

                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public ActionResult<NoteResult> AddNote([Required][FromBody] NoteRequestModel model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var note = _notesService.AddNote(model.Title, model.Body, userId, model.SharedUsersData);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Successfully added note[{note.Id}] of user[{userId}]");

                return Ok(new NoteResult
                {
                    Body = note.Body,
                    Id = note.Id,
                    Title = note.Title,
                    SharedUsersData = new List<SharingData>(_sharesService.GetShares(note.Id).Select(s => new SharingData()
                    {
                        Level = s.Level,
                        UserId = s.UserId,
                        Username = _usersService.GetUsernameByUserId(s.UserId)
                    })).ToArray()
                });
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{noteId}")]
        public IActionResult DeleteNote([Required] int noteId)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                _notesService.DeleteNote(noteId, userId);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Successfully deleted note[{noteId}] of user[{userId}]");

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
        public IActionResult NoteCount()
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var count = _notesService.GetNoteCount(userId);

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
        public IActionResult NoteCountFiltered([Required][FromBody] SearchRequestModel model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var count = _notesService.GetNoteCount(userId, model.Search);

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
