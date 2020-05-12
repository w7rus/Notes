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
using Notes.Logic.Common;
using Notes.Logic.Services.Attachments;
using Notes.Logic.Services.Notes;
using Notes.Logic.Services.Shares;
using Serilog;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicAttachmentController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;
        private readonly INotesService _notesService;
        private readonly ISharesService _sharesService;

        public PublicAttachmentController(IAttachmentService attachmentService, 
            INotesService notesService, ISharesService sharesService)
        {
            _attachmentService = attachmentService;
            _notesService = notesService;
            _sharesService = sharesService;
        }

        //Create Attachment
        [HttpPost("{noteId}"), DisableRequestSizeLimit]
        public async Task<ActionResult> AddAttachment([Required] int noteId)
        {
            try
            {
                var userId = 1;

                var note = await _notesService.GetSharedNote(noteId);
                var sharedUsersData = _sharesService.GetShares(noteId);
                var sharedUserData = sharedUsersData.FirstOrDefault(s => s.UserId == userId);
                if (sharedUserData == null)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }
                if (sharedUserData.Level < SharingLevels.Level.ReadWrite)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have READWRITE permissions to operate with note[{note.UserId}]");
                    return BadRequest();
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

        //List Attachments
        [HttpGet("findAttachments/{noteId}")]
        public async Task<ActionResult> GetAttachments([Required] int noteId)
        {
            try
            {
                var userId = 1;

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
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have READ permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending attachments ids of note[{note.Id}]");

                var ids = await _attachmentService.GetAttachmentList(noteId);
                return Ok(ids);
            }
            catch (Exception e)
            {
                Log.Error(e, $"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] " + e.Message);
                return BadRequest(e.Message);
            }
        }

        //Delete Attachment
        [HttpDelete("{noteId}/{attachmentId}")]
        public async Task<ActionResult> DeleteAttachment([Required] int noteId, [Required] int attachmentId)
        {
            try
            {
                var userId = 1;

                var note = await _notesService.GetSharedNote(noteId);
                var sharedUsersData = _sharesService.GetShares(noteId);
                var sharedUserData = sharedUsersData.FirstOrDefault(s => s.UserId == userId);
                if (sharedUserData == null)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }
                if (sharedUserData.Level < SharingLevels.Level.ReadWrite)
                {
                    Log.Warning($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] User[{userId}] does not have READWRITE permissions to operate with note[{note.UserId}]");
                    return BadRequest();
                }

                Log.Information($"[{Request.Path}:{Request.Method}/{HttpContext.Connection.RemoteIpAddress}] Sending attachments ids of note[{note.Id}]");

                await _attachmentService.DeleteAttachment(attachmentId);

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