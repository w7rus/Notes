using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Services.Attachments
{
    public interface IAttachmentService
    {
        public Task<Attachment> AddAttachment(int noteId, byte[] fileData);
    }
}
