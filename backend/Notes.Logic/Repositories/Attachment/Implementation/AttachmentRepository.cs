using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Notes.Logic.Data;
using Notes.Logic.Models;

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

        public async Task<Models.Database.Attachment> GetAttachment(int attachmentId)
        {
            return await _context.Attachments.FindAsync(attachmentId);
        }

        public async Task<ICollection<AttachmentResult>> GetAttachmentsList(int noteId)
        {
            return await _context.Attachments.Where(a => a.NoteId == noteId).Select(a => new AttachmentResult
            {
                Id = a.Id,
                Filename = a.Filename
            }).ToListAsync();
        }

        public async Task DeleteAttachment(Models.Database.Attachment attachment)
        {
            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
        }

        public ICollection<Models.Database.Attachment> GetAttachments(int noteId)
        {
            return _context.Attachments
                .Where(a => a.NoteId == noteId).ToArray();
        }

        public async Task DeleteAttachments(ICollection<Models.Database.Attachment> attachments)
        {
            _context.Attachments.RemoveRange(attachments);
            await _context.SaveChangesAsync();
        }
    }
}
