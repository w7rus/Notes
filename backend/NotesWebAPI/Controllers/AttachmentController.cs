using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Logic.Services.Attachments;
using Notes.Logic.Services.Notes;
using Serilog;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;
        private readonly INotesService _notesService;

        public AttachmentController(IAttachmentService attachmentService, 
            INotesService notesService)
        {
            _attachmentService = attachmentService;
            _notesService = notesService;
        }

        //Create Attachment
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{noteId}"), DisableRequestSizeLimit]
        public async Task<ActionResult> AddAttachment([Required] int noteId)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var file = Request.Form.Files[0];

                if (file.Length <= 0) return NoContent();

                var fileExtension = new FileInfo(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')).Extension;

                await using var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);

                var fileData = ms.ToArray();
                var attachment = await _attachmentService.AddAttachment(noteId, fileData, fileExtension);

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Successfully added attachment[{attachment.Id}] of user[{userId}]");

                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        //Read Attachment
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{attachmentId}")]
        public async Task<ActionResult> ReadAttachment([Required] int attachmentId)
        {
            try
            {
                if (!TryGetUserId(out var userId))
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Required field \"user_id\" is not found in JWT from");
                    return Unauthorized();
                }

                var attachment = await _attachmentService.GetAttachment(attachmentId);
                var note = await _notesService.GetNote(attachment.NoteId);

                if (note.UserId != userId)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending attachment[{attachmentId}] of user[{userId}]");

                var fileData = await _attachmentService.GetAttachmentFile(attachment.Filename);
                var stream = new MemoryStream(fileData);

                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();

                return new FileStreamResult(stream, "application/octet-stream")
                {
                    FileDownloadName = attachment.Filename
                };
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        //Delete Attachment

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