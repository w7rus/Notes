using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesWebAPI.Data;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesWebAPIContext _context;

        public NotesController(NotesWebAPIContext context)
        {
            _context = context;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult<IEnumerable<Models.Database.Note>> NoteList()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            return _context.Notes.Where(n => n.UserId == userId).ToList();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public ActionResult<Models.Database.Note> GetNote([Required] int id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var note = _context.Notes.Find(id);

            if (note.UserId != userId)
                return BadRequest();

            return note;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public IActionResult UpdateNote([Required] int id, [Required][FromBody] Models.View.NoteRequestModel model)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var note = _context.Notes.Find(id);

            if (note == null)
                return BadRequest();

            if (note.UserId != userId)
                return BadRequest();

            note.Title = model.Title;
            note.Body = model.Body;

            _context.Notes.Update(note);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public ActionResult<Models.Database.Note> PostNote([Required][FromBody] Models.View.NoteRequestModel model)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var note = new Models.Database.Note
            {
                UserId = userId,
                Title = model.Title,
                Body = model.Body
            };

            _context.Notes.Add(note);
            _context.SaveChanges();

            return Ok(note);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public ActionResult<Models.Database.Note> DeleteNote([Required] int id)
        {
            var note = _context.Notes.Find(id);

            if (note == null)
                return NotFound();

            if (!TryGetUserId(out var userId))
                return Unauthorized();

            if (note.UserId != userId)
                return BadRequest();

            _context.Notes.Remove(note);
            _context.SaveChanges();

            return Ok();
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
