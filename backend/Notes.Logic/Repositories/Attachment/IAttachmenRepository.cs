using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notes.Logic.Models;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Attachment
{
    public interface IAttachmentRepository
    {
        public Task<Models.Database.Attachment> AddAttachment(Models.Database.Attachment attachment);
        public Task<Models.Database.Attachment> GetAttachment(int attachmentId);
        public Task<ICollection<AttachmentResult>> GetAttachmentsList(int noteId);

        public Task DeleteAttachment(Models.Database.Attachment attachment);
        public ICollection<Models.Database.Attachment> GetAttachments(int noteId);
        public Task DeleteAttachments(ICollection<Models.Database.Attachment> attachments);
    }
}
