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
using NotesWebAPI.Models.View.Request;
using Serilog;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicNotesController : ControllerBase
    {
        private readonly INotesService _notesService;
        private readonly ISharesService _sharesService;

        public PublicNotesController(INotesService notesService, ISharesService sharesService)
        {
            _notesService = notesService;
            _sharesService = sharesService;
        }

        //Read Shared Note
        [HttpGet("{noteId}")]
        public async Task<ActionResult<NoteResult>> GetSharedNote([Required] int noteId)
        {
            try
            {
                var note = await _notesService.GetSharedNote(noteId);

                var sharedUsersData = _sharesService.GetShares(noteId);

                var sharedUserData = sharedUsersData.FirstOrDefault(s => s.UserId == 1);

                if (sharedUserData == null)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Anonymous user does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                if (sharedUserData.Level < SharingLevels.Level.Read)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Anonymous user does not have READ/READWRITE permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending shared note[{note.Id}] to anonymous user");

                return Ok(new NoteResult
                {
                    Body = note.Body,
                    Id = note.Id,
                    Title = note.Title
                });
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        //Update Shared Note
        [HttpPut("{noteId}")]
        public async Task<ActionResult> UpdateSharedNote([Required] int noteId, [Required][FromBody] DashboardNoteRequest model)
        {
            try
            {
                await _notesService.UpdateSharedNote(noteId, model.Title, model.Body, 1);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Successfully updated shared note[{noteId}] of anonymous user");

                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }



        //Other
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        // [HttpGet]
        // public async Task<ActionResult<ICollection<NoteResult>>> SharedNoteList()
        // {
        //     try
        //     {
        //         if (!TryGetUserId(out var userId))
        //         {
        //             Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
        //             return Unauthorized();
        //         }
        //
        //         var notes = (await _notesService.ListSharedNotes(userId)).ToList();
        //
        //         Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending {notes.Count()} shared notes[{string.Join(", ", notes.Select(n => n.Id))}] to user[{userId}]");
        //
        //         return Ok(notes.Select(n =>
        //             new NoteResult
        //             {
        //                 Body = n.Body,
        //                 Id = n.Id,
        //                 Title = n.Title
        //             }
        //         ).ToArray());
        //     }
        //     catch (Exception e)
        //     {
        //         Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
        //         return BadRequest(e.Message);
        //     }
        // }

        [HttpPost("list")]
        public async Task<ActionResult<ICollection<NoteResult>>> SharedNoteListFiltered([Required][FromBody] DashboardNoteFilterRequest model)
        {
            try
            {
                var notes = (await _notesService.ListSharedNotes(1, model.Search, model.Sorting, model.Display, model.Page)).ToList();

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending {notes.Count()} shared notes[{string.Join(", ", notes.Select(n => n.Id))}] filters:[\"{model.Search}\", {model.Sorting}, {model.Display}, {model.Page}] to anonymous user");

                return Ok(notes.Select(n =>
                    new NoteResult
                    {
                        Body = n.Body,
                        Id = n.Id,
                        Title = n.Title
                    }
                ).ToArray());
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> SharedNoteCount()
        {
            try
            {
                var count = await _notesService.GetSharedNoteCount(1);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending int[{count}] to anonymous user");

                return Ok(count);
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("countFiltered")]
        public async Task<ActionResult<int>> SharedNoteCountFiltered([Required] [FromBody] DashboardNoteFilterRequest model)
        {
            try
            {
                var count = await _notesService.GetSharedNoteCount(1, model.Search);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending int[{count}] filters:[\"{model.Search}\"] to anonymous user");

                return Ok(count);
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}