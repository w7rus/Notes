using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Notes.Logic.Models;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Services.Attachments
{
    public interface IAttachmentService
    {
        public Task<Attachment> AddAttachment(int noteId, byte[] fileData, string fileExtension);
        public Task<Attachment> GetAttachment(int attachmentId);
        public ICollection<Attachment> GetAttachments(int noteId);
        public Task<byte[]> GetAttachmentFile(string fileName);
        public Task<ICollection<AttachmentResult>> GetAttachmentList(int noteId);
        public Task DeleteAttachment(int attachmentId);
        public Task DeleteAttachmentsForNote(int noteId);
    }
}
