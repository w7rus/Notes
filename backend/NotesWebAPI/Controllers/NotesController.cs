using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Logic.Services.Notes;
using NotesWebAPI.Models.View.Request;
using NotesWebAPI.Models.View.Response;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;

        public NotesController(INotesService notesServiceService)
        {
            _notesService = notesServiceService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult<IEnumerable<NoteResponseModel>> NoteList()
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized();

                return new ActionResult<IEnumerable<NoteResponseModel>>(_notesService.ListNotes(userId).Select(n =>
                    new NoteResponseModel
                    {
                        Body = n.Body,
                        Id = n.Id,
                        Title = n.Title
                    }
                ));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public ActionResult<NoteResponseModel> GetNote([Required] int id)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized();

                var note = _notesService.GetNote(id);

                if (note.UserId != userId)
                    return BadRequest();

                return new NoteResponseModel
                {
                    Body = note.Body,
                    Id = note.Id,
                    Title = note.Title
                };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public IActionResult UpdateNote([Required] int id, [Required][FromBody] NoteRequestModel model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized();

                _notesService.UpdateNote(id, model.Title, model.Body, userId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public ActionResult<NoteResponseModel> PostNote([Required][FromBody] NoteRequestModel model)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized();

                var note = _notesService.AddNote(model.Title, model.Body, userId);

                return Ok(new NoteResponseModel
                {
                    Body = note.Body,
                    Id = note.Id,
                    Title = note.Title
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public IActionResult DeleteNote([Required] int id)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                    return Unauthorized();

                _notesService.DeleteNote(id, userId);

                return Ok();
            }
            catch (Exception e)
            {
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
