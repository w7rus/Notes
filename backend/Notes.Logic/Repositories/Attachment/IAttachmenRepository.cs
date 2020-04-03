using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Repositories.Attachment
{
    public interface IAttachmentRepository
    {
        public Task<Models.Database.Attachment> AddAttachment(Models.Database.Attachment attachment);
    }
}
