using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notes.Logic.Data;

namespace Notes.Logic.Repositories.Attachment.Implementation
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly NotesWebAPIContext _context;

        public AttachmentRepository(NotesWebAPIContext context)
        {
            _context = context;
        }

        public async Task<Models.Database.Attachment> AddAttachment(Models.Database.Attachment attachment)
        {
            await _context.Attachments.AddAsync(attachment);
            await _context.SaveChangesAsync();

            return attachment;
        }
    }
}
