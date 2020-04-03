using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Logic.Services.Attachments;
using Serilog;

namespace NotesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        //Create Attachment
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{noteId}"), DisableRequestSizeLimit]
        public async Task<ActionResult> AddNote([Required] int noteId)
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

                await using var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);

                var fileData = ms.ToArray();
                var attachment = await _attachmentService.AddAttachment(noteId, fileData);

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